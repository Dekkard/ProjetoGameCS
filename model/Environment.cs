using LiteDB;
public class Environment
{
    private int _id;
    private EnvType _type;
    private EnvTwistType _typeTwist;

    public int Id { get => _id; set => _id = value; }
    public EnvType Type { get => _type; set => _type = value; }
    public EnvTwistType TypeTwist { get => _typeTwist; set => _typeTwist = value; }

    public Environment() { _id = ObjectId.NewObjectId().Increment; }

    public Environment(string name, EnvType type, EnvTwistType typeTwist)
    {
        _id = ObjectId.NewObjectId().Increment;
        _type = type;
        _typeTwist = typeTwist;
    }
}
public enum EnvType
{
    Florest, Mountain, Deep_Canyon, Swamp, Cave, Abandoned_Castle, Old_Battlefield, Old_Temple, Cove, Old_Port, Shipwreck, Cemitery,
}
public enum EnvTwistType
{
    Paradise, Secret_Garden, Dreamland, Abyss, Hell, Old_Factory, Secret_Base,
}
public enum EnvTypePt
{
    Floresta, Montanha, Ravina_Profunda, Pântano, Caverna, Castelo_Abandonado, Campo_de_Batalha_Antigo, Templo_Antigo, Enseada, Porto_Antigo, Navio_Arruinado, Cemitério,
}
public enum EnvTwistTypePt
{
    Paraíso, Jardim_Secreto, Terrasonho, Abismo, Inferno, Fábrica_Antiga, Base_Secreta,
}