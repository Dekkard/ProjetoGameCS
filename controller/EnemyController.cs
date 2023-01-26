using static BaseService;
#pragma warning disable 8600, 8601, 8602, 8603, 8604, 8605, 8625
public class EnemyController
{
    
    private Enemy enemy;
    public InventoryController inventoryController;
    public EquipController equipController;

    public Enemy Enemy { get => enemy; }
    public EnemyController(LiteDB.LiteDatabase db, Enemy enemy)
    {
        this.enemy = enemy;
        inventoryController = new InventoryController(db, enemy.Id, enemy.Equip);
        equipController = new EquipController(db, enemy.Equip);
    }
   
    //private LiteDB.LiteDatabase _db;
    public void GenerateEnemy(int level, int rarity, int quality, int aiLvl, LiteDB.LiteDatabase db, int[] enemyTypeList = null)
    {
        Enemy enemy = new Enemy();
        Random rng = new Random();

        // int[] status = { 0, 0, 0, 5, 0, 0, 5 };
        int ptsStatus = 25 + (level - 1) * 2;

        enemy.Strength = ptsStatus * rng.Next(5);
        enemy.Perception = ptsStatus * rng.Next(5);
        enemy.Endurance = ptsStatus * rng.Next(5);
        enemy.Charisma = ptsStatus * rng.Next(5);
        enemy.Intelligence = ptsStatus * rng.Next(5);
        enemy.Agility = ptsStatus * rng.Next(5);
        enemy.Luck = ptsStatus * rng.Next(5);
        enemy.TotalHitPoints();
        enemy.TotalManaPoints();

        enemy.Riches = Value.GenerateValue(rng, rarity, quality);

        int minValue = (level - 5) < 1 ? 1 : level - 5;
        int maxValue = (level + 5) > LEVELCAP ? LEVELCAP : level + 5;
        enemy.Level = rng.Next(minValue, maxValue);
        enemy.IaLevel = rng.Next(LEVELCAP) > LEVELCAP - enemy.Level ? 6 : EnemyAIRng(rng, enemy.Level, 5, 3);
        int enemyType;
        if (enemyTypeList != null)
        {
            enemyType = enemyTypeList[rng.Next(enemyTypeList.Length)];
        }
        else
        {
            enemyType = rng.Next(Enum.GetValues(typeof(EnemyType)).Length);
        }

        // Console.Write((EnemyType)enemyType + "[" + enemyType + "]: ");
        Type type = Type.GetType(Enum.GetName(typeof(EnemyType), enemyType).ToString() + "Name");
        string name = Enum.GetName(type, rng.Next(Enum.GetValues(type).Length));
        switch (enemyType)
        {
            case 0:
                name += " " + Enum.GetName(typeof(HumanoidRank), EnemylLevelingRng(level, rarity, quality, 0.7, 0.7, 0.7, 1.0, typeof(HumanoidRank)));
                int[] meleeItemList = new int[] { 21, 22, 23, 33, 41, 42, 24, 25, 26, 27, 28 };
                // int[] ringItemList = new int[] 
                for (int i = 1; i <= 14; i++)
                {
                    if (rng.Next(10) >= 7)
                    {
                        Item item;
                        if (i <= 9 || i == 12)
                        {
                            item = InventoryController.GerarItem(enemy.Id, minValue, maxValue, rarity, quality, i);
                        }
                        else if (i == 13)
                        {
                            item = InventoryController.GerarItem(enemy.Id, minValue, maxValue, rarity, quality, meleeItemList[rng.Next(meleeItemList.Length)]);
                        }
                        else if (i == 10 || i == 11)
                        {
                            item = InventoryController.GerarItem(enemy.Id, minValue, maxValue, rarity, quality, 11);
                        }
                        else
                        {
                            if (rng.Next(10) >= 7)
                            {
                                item = InventoryController.GerarItem(enemy.Id, minValue, maxValue, rarity, quality, 13);
                            }
                            else
                            {
                                item = InventoryController.GerarItem(enemy.Id, minValue, maxValue, rarity, quality, meleeItemList[rng.Next(meleeItemList.Length)]);
                            }
                        }
                        inventoryController.AddItem(item);
                        equipController.ChangeEquip(item, i);
                    }
                }
                break;
            default:
                Type type2 = Type.GetType(Enum.GetName(typeof(EnemyType), enemyType).ToString() + "Rank");
                name += " " + Enum.GetName(type2, EnemylLevelingRng(level, rarity, quality, 0.7, 0.7, 0.7, 1.0, type2));
                break;
        }
        enemy.Name = name.Replace("_", " ");
        this.enemy = enemy;
    }
    public static int EnemylLevelingRng(int level, int rarity, int quality, double levelrates, double rarityRates, double qualityRates, double capRates, Type t)
    {
        Random rng = new Random();
        int et = Enum.GetValues(t).Length - 1;
        double d = (((level * et) + rng.Next((int)(rarity + quality) / 2)) / (LEVELCAP)) * 1.5;
        int n = (int)(Math.Floor(d > et ? et : (d < 0 ? 0 : d)));
        level = (int)Math.Pow(level, levelrates);
        rarity = (int)Math.Pow(rarity, rarityRates);
        quality = (int)Math.Pow(quality, qualityRates);
        int cap = (int)Math.Pow(LEVELCAP, 3 * capRates);
        int chance = cap - LEVELCAP;
        double capRed = Math.Pow(cap - 1000, 1 / et);
        double chanceRed = Math.Pow(chance - 100, 1 / et);
        return rng.Next(cap) > chance - (level * rarity * quality) ? n : ELRHelper(rng, level, rarity, quality, (int)(cap / capRed), (int)(chance / chanceRed), n - 1, capRed, chanceRed);
    }
    public static int ELRHelper(Random rng, int level, int rarity, int quality, int cap, int chance, int num, double capRed, double chanceRed)
    {
        if (num <= 0)
            return 0;
        else
            return rng.Next(cap) > chance - (level * rarity * quality) ? num : ELRHelper(rng, level, rarity, quality, (int)(cap / capRed), (int)(chance / chanceRed), num - 1, capRed, chanceRed);
    }
    public static int EnemyAIRng(Random rng, int level, int ailvl, int factor)
    {
        if (ailvl > 1)
        {
            return rng.Next(LEVELCAP) > LEVELCAP - (level * factor) ? ailvl : EnemyAIRng(rng, level, ailvl - 1, factor * 3);
        }
        else
        {
            return 1;
        }
    }
}