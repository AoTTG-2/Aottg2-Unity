//Copyright (c) 2016-2018 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; //converting arrays to dictionaries
using System.IO; //for getting folders

[CreateAssetMenu(fileName = "New Text Data", menuName = "Super Text Mesh/Super Text Mesh Data", order = 0)]
public class SuperTextMeshData : ScriptableObject { //the actual textdata manager file
	
	//[HideInInspector] public bool textDataEditMode = false; //whether this will show on objects or not
	
	[HideInInspector] public bool showEffectsFoldout = false;

	[HideInInspector] public bool showWavesFoldout = false;
	public Dictionary<string,STMWaveData> waves = new Dictionary<string,STMWaveData>();
	[HideInInspector] public bool showJittersFoldout = false;
	public Dictionary<string,STMJitterData> jitters = new Dictionary<string,STMJitterData>();
	[HideInInspector] public bool showDrawAnimsFoldout = false;
	public Dictionary<string,STMDrawAnimData> drawAnims = new Dictionary<string,STMDrawAnimData>();


	[HideInInspector] public bool showTextColorFoldout = false;

	[HideInInspector] public bool showColorsFoldout = false;
	public Dictionary<string,STMColorData> colors = new Dictionary<string,STMColorData>();
	[HideInInspector] public bool showGradientsFoldout = false;
	public Dictionary<string,STMGradientData> gradients = new Dictionary<string,STMGradientData>();
	[HideInInspector] public bool showTexturesFoldout = false;
	public Dictionary<string,STMTextureData> textures = new Dictionary<string,STMTextureData>();


    [HideInInspector] public bool showInlineFoldout = false;

    [HideInInspector] public bool showDelaysFoldout = false;
    public Dictionary<string,STMDelayData> delays = new Dictionary<string,STMDelayData>();
    [HideInInspector] public bool showVoicesFoldout = false;
    public Dictionary<string,STMVoiceData> voices = new Dictionary<string,STMVoiceData>();
    [HideInInspector] public bool showFontsFoldout = false;
    public Dictionary<string,STMFontData> fonts = new Dictionary<string,STMFontData>();
    [HideInInspector] public bool showSoundClipsFoldout = false;
    public Dictionary<string,STMSoundClipData> soundClips = new Dictionary<string,STMSoundClipData>();
    //public List<bool> showSoundClipFoldout = new List<bool>();
    [HideInInspector] public bool showAudioClipsFoldout = false;
    public Dictionary<string,STMAudioClipData> audioClips = new Dictionary<string,STMAudioClipData>();
    [HideInInspector] public bool showQuadsFoldout = false;
    public Dictionary<string,STMQuadData> quads = new Dictionary<string,STMQuadData>();
    [HideInInspector] public bool showMaterialsFoldout = false;
    public Dictionary<string,STMMaterialData> materials = new Dictionary<string,STMMaterialData>();


    [HideInInspector] public bool showAutomaticFoldout = false;

    [HideInInspector] public bool showAutoClipsFoldout = false;
    public Dictionary<string,STMAutoClipData> autoClips = new Dictionary<string,STMAutoClipData>();
    [HideInInspector] public bool showAutoDelaysFoldout = false;
    public Dictionary<string,STMAutoDelayData> autoDelays = new Dictionary<string,STMAutoDelayData>();


    [HideInInspector] public bool showMasterFoldout = true;

	[Tooltip("This disables waves and jitters from effecting text position, which might be hard for some users to read.")]
    public bool disableAnimatedText = false;
	public Font defaultFont;
	public Color boundsColor = Color.blue;
	public Color textBoundsColor = Color.yellow;
	public Color finalTextBoundsColor = Color.grey;
	public float superscriptOffset = 0.5f;
	public float superscriptSize = 0.5f;
	public float subscriptOffset = -0.2f;
	public float subscriptSize = 0.5f;
	public Font inspectorFont;



    public void RebuildDictionaries(){
    	waves = Resources.LoadAll<STMWaveData>("STMWaves").ToDictionary(x => x.name, x => x);
    	jitters = Resources.LoadAll<STMJitterData>("STMJitters").ToDictionary(x => x.name, x => x);
    	drawAnims = Resources.LoadAll<STMDrawAnimData>("STMDrawAnims").ToDictionary(x => x.name, x => x);

    	colors = Resources.LoadAll<STMColorData>("STMColors").ToDictionary(x => x.name, x => x);
    	gradients = Resources.LoadAll<STMGradientData>("STMGradients").ToDictionary(x => x.name, x => x);
    	textures = Resources.LoadAll<STMTextureData>("STMTextures").ToDictionary(x => x.name, x => x);

    	delays = Resources.LoadAll<STMDelayData>("STMDelays").ToDictionary(x => x.name, x => x);
    	voices = Resources.LoadAll<STMVoiceData>("STMVoices").ToDictionary(x => x.name, x => x);
    	fonts = Resources.LoadAll<STMFontData>("STMFonts").ToDictionary(x => x.name, x => x);

    	soundClips = Resources.LoadAll<STMSoundClipData>("STMSoundClips").ToDictionary(x => x.name, x => x);
    	audioClips = Resources.LoadAll<STMAudioClipData>("STMAudioClips").ToDictionary(x => x.name, x => x);
    	quads = Resources.LoadAll<STMQuadData>("STMQuads").ToDictionary(x => x.name, x => x);
    	materials = Resources.LoadAll<STMMaterialData>("STMMaterials").ToDictionary(x => x.name, x => x);
	    
	    //make sure that they have distinct values!
    	autoClips = Resources.LoadAll<STMAutoClipData>("STMAutoClips").GroupBy(x => x.type == STMAutoClipData.Type.Quad ? x.quadName : x.character.ToString()).Select(x => x.First()).ToDictionary(x => x.type == STMAutoClipData.Type.Quad ? x.quadName : x.character.ToString(), x => x);
    	autoDelays = Resources.LoadAll<STMAutoDelayData>("STMAutoDelays").GroupBy(x => x.type == STMAutoDelayData.Type.Quad ? x.quadName : x.character.ToString()).Select(x => x.First()).ToDictionary(x => x.type == STMAutoDelayData.Type.Quad ? x.quadName : x.character.ToString(), x => x);

    }
}
