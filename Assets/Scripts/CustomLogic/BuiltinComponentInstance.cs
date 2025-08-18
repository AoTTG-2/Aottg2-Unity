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

        // Utility method to check and add component if needed
        protected static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            T existing = gameObject.GetComponent<T>();
            return existing != null ? existing : gameObject.AddComponent<T>();
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
