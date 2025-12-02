namespace Prototype
{
    public static class Paths
    {
        public static class ScriptableObjects
        {
            private const string BASE_PATH = "Prototype/";
            
            public static class PlayerActions
            {
                private const string NAME = "PlayerActionsSO";
                
                public const string FileName = NAME;
                public const string MenuName = BASE_PATH + NAME;
            }

            public static class Equipment
            {
                private const string NAME = "EquipmentSO";
                
                public const string FileName = NAME;
                public const string MenuName = BASE_PATH + NAME;
            }
            
            public static class TimerSettings
            {
                private const string NAME = "TimerSettingsSO";
                
                public const string FileName = NAME;
                public const string MenuName = BASE_PATH + NAME;
            }
        }
    }
}
