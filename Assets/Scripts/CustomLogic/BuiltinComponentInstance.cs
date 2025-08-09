using UnityEngine;


namespace CustomLogic
{
    abstract class BuiltinComponentInstance : BuiltinClassInstance
    {
        // force implementing class to pass down a Component in the constructor
        public Component Component;
        protected BuiltinComponentInstance(Component component)
        {
            this.Component = component;
        }

        public bool Enabled
        {
            get => Component is Behaviour behaviour && behaviour.enabled;
            set
            {
                if (Component is Behaviour behaviour)
                {
                    behaviour.enabled = value;
                }
            }
        }

        public void Unload()
        {
            // destroy
            if (Component != null)
            {
                Object.Destroy(Component);
                Component = null;
            }
        }
    }
}
