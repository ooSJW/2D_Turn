namespace project02
{
    public enum SceneName
    {
        InitializeScene,
        MainLobbyScene,
        CombatScene,
        LoadingScene,
    }
    public enum PoolObject
    {
        HpParticle,
    }
    public enum CombatState
    {
        None,
        Start,
        PlayerTurn,
        EnemyTurn,
        GoNextStage,
        AllStageClear,
        PlayerLose,
    }

    public enum KnightName
    {
        Alex = 1,
        Steve,
        Sandy,
    }

    public enum ItemName
    {
        WoodenClub = 1,
        OldIronSword = 2,
        FineIronSword = 3,
        WoodenArmor = 11,
        OldIronArmor = 12,
        FineIronArmor = 13,
    }

    public enum KnightState
    {
        Idle,
        GoForward,
        Return,
        GoNextStage,
        Attack,
        UseSkill,
        GetDamage,
        Death,
    }

    public enum SkillName
    {
        None,
        AlexSkill,
        SteveSkill,
        SandySkill,
    }
    public enum SkillType
    {
        Attack,
        Heal,
    }

    public enum CombatObjectAnimationParam
    {
        State,
    }
    public enum EnemyName
    {
        Goblin = 11,
        Ogre = 12,
        BossOrc = 13,
        StoneGolem = 14,
        SandGolem = 15,
        BossGolem = 16,
    }

    public enum EnemyState
    {
        Idle,
        GoForward,
        Return,
        Attack,
        GetDamage,
        Death,
    }

    public enum EnemyType
    {
        Normal,
        Boss,
    }

    public enum RewardType
    {
        Knight,
        Item,
    }

    public enum ItemType
    {
        None,
        Weapon,
        Armor,
    }

    public enum AudioClipName
    {
        Sfx_Click,
        Sfx_Attack01,
        Sfx_Attack02,
        Sfx_AlexSkill,
        Sfx_SteveSkill,
        Sfx_SandySkill,
        Sfx_Star,
        Sfx_PlayerLose,
        Sfx_Reinforce,
    }
    public enum GraphicQuality
    {
        High,
        Medium,
        Low,
    }
}
