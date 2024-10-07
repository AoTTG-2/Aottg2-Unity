using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Weather
{
    class WeatherScheduleRunner
    {
        const int ScheduleMaxRecursion = 200;
        int _currentScheduleLine;
        LinkedList<int> _callerStack = new LinkedList<int>();
        Dictionary<string, int> _scheduleLabels = new Dictionary<string, int>();
        Dictionary<int, int> _repeatStartLines = new Dictionary<int, int>();
        Dictionary<int, int> _repeatCurrentCounts = new Dictionary<int, int>();
        WeatherManager _manager;
        public WeatherSchedule Schedule = new WeatherSchedule();

        public WeatherScheduleRunner(WeatherManager manager)
        {
            _manager = manager;
        }

        public void ProcessSchedule()
        {
            for (int i = 0; i < Schedule.Events.Count - 1; i++)
            {
                if (Schedule.Events[i].Action == WeatherAction.RepeatNext)
                {
                    Schedule.Events[i].Action = WeatherAction.BeginRepeat;
                    WeatherEvent ev = new WeatherEvent
                    {
                        Action = WeatherAction.EndRepeat
                    };
                    Schedule.Events.Insert(i + 2, ev);
                }
            }
            int lastRepeatLine = -1;
            for (int i = 0; i < Schedule.Events.Count; i++)
            {
                WeatherEvent ev = Schedule.Events[i];
                if (ev.Action == WeatherAction.Label)
                {
                    string labelName = (string)ev.GetValue();
                    if (!_scheduleLabels.ContainsKey(labelName))
                        _scheduleLabels.Add(labelName, i);
                }
                else if (ev.Action == WeatherAction.BeginRepeat)
                {
                    lastRepeatLine = i;
                    _repeatCurrentCounts.Add(i, 0);
                }
                else if (ev.Action == WeatherAction.EndRepeat)
                {
                    if (lastRepeatLine >= 0)
                    {
                        _repeatStartLines.Add(i, lastRepeatLine);
                        lastRepeatLine = -1;
                    }
                }
            }
        }

        public void ConsumeSchedule()
        {
            int count = 0;
            bool encounteredWait = false;
            while (!encounteredWait)
            {
                count += 1;
                if (count > ScheduleMaxRecursion)
                {
                    DebugConsole.Log("Weather schedule reached max usage (did you forget to use waits?)", true);
                    Schedule.Events.Clear();
                    return;
                }
                if (Schedule.Events.Count == 0 || _currentScheduleLine < 0 || _currentScheduleLine >= Schedule.Events.Count)
                    return;
                WeatherEvent ev = Schedule.Events[_currentScheduleLine];
                switch (ev.Action)
                {
                    case WeatherAction.SetDefaultAll:
                        bool loop = _manager._currentWeather.ScheduleLoop.Value;
                        _manager._currentWeather.SetDefault();
                        _manager._currentWeather.UseSchedule.Value = true;
                        _manager._currentWeather.ScheduleLoop.Value = loop;
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetDefault:
                        ((BaseSetting)_manager._currentWeather.Settings[(ev.Effect).ToString()]).SetDefault();
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetValue:
                        BaseSetting currentSetting = (BaseSetting)_manager._currentWeather.Settings[(ev.Effect).ToString()];
                        SettingsUtil.SetSettingValue(currentSetting, ev.GetSettingType(), ev.GetValue());
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetTargetDefaultAll:
                        _manager._targetWeather.SetDefault();
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetTargetDefault:
                        ((BaseSetting)_manager._targetWeather.Settings[(ev.Effect).ToString()]).SetDefault();
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetTargetValue:
                        BaseSetting targetSetting = (BaseSetting)_manager._targetWeather.Settings[(ev.Effect).ToString()];
                        SettingsUtil.SetSettingValue(targetSetting, ev.GetSettingType(), ev.GetValue());
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetTargetTime:
                        if (!_manager._targetWeatherStartTimes.ContainsKey((int)ev.Effect))
                        {
                            _manager._targetWeatherStartTimes.Add((int)ev.Effect, 0f);
                            _manager._targetWeatherEndTimes.Add((int)ev.Effect, 0f);
                        }
                        BaseSetting start = (BaseSetting)_manager._startWeather.Settings[(ev.Effect).ToString()];
                        BaseSetting current = (BaseSetting)_manager._currentWeather.Settings[(ev.Effect).ToString()];
                        _manager._targetWeatherStartTimes[(int)ev.Effect] = _manager._currentTime;
                        _manager._targetWeatherEndTimes[(int)ev.Effect] = _manager._currentTime + (float)ev.GetValue();
                        start.Copy(current);
                        _manager._needSync = true;
                        break;
                    case WeatherAction.SetTargetTimeAll:
                        _manager._targetWeatherStartTimes.Clear();
                        _manager._targetWeatherEndTimes.Clear();
                        float endTime = _manager._currentTime + (float)ev.GetValue();
                        foreach (WeatherEffect effect in Util.EnumToList<WeatherEffect>())
                        {
                            _manager._targetWeatherStartTimes.Add((int)effect, _manager._currentTime);
                            _manager._targetWeatherEndTimes.Add((int)effect, endTime);
                        }
                        _manager._needSync = true;
                        break;
                    case WeatherAction.Wait:
                        _manager._currentScheduleWait[this] = (float)ev.GetValue();
                        encounteredWait = true;
                        break;
                    case WeatherAction.Goto:
                        string labelName = (string)ev.GetValue();
                        if (labelName != "NextLine" && _scheduleLabels.ContainsKey(labelName))
                        {
                            _callerStack.AddLast(_currentScheduleLine);
                            if (_callerStack.Count > ScheduleMaxRecursion)
                                _callerStack.RemoveFirst();
                            _currentScheduleLine = _scheduleLabels[labelName];
                        }
                        break;
                    case WeatherAction.Return:
                        if (_callerStack.Count > 0)
                        {
                            _currentScheduleLine = _callerStack.Last.Value;
                            _callerStack.RemoveLast();
                        }
                        break;
                    case WeatherAction.BeginRepeat:
                        _repeatCurrentCounts[_currentScheduleLine] = (int)ev.GetValue();
                        break;
                    case WeatherAction.EndRepeat:
                        int beginRepeatLine = _repeatStartLines[_currentScheduleLine];
                        if (_repeatCurrentCounts.ContainsKey(beginRepeatLine) && _repeatCurrentCounts[beginRepeatLine] > 0)
                        {
                            _currentScheduleLine = beginRepeatLine + 1;
                            _repeatCurrentCounts[beginRepeatLine] -= 1;
                        }
                        break;
                }
                _currentScheduleLine += 1;
                if (_currentScheduleLine >= Schedule.Events.Count && _manager._currentWeather.ScheduleLoop.Value)
                    _currentScheduleLine = 0;
            }
        }
    }
}
