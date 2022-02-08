using static BaseController;
using LiteDB;
public class EquipController
{
    /* private int _heroId;

    public int HeroId { get => _heroId; set => _heroId = value; }
    public EquipController(int id){_heroId = id;} */
    public static int GetEquipSlot(int slot, Equip equip)
    {
        switch (slot)
        {
            case 1:
                return equip.Head;
            case 2:
                return equip.Body;
            case 3:
                return equip.Shoulder;
            case 4:
                return equip.Wrist;
            case 5:
                return equip.Hand;
            case 6:
                return equip.Legs;
            case 7:
                return equip.Feet;
            case 8:
                return equip.Cape;
            case 9:
                return equip.Belt;
            case 10:
                return equip.RightRing;
            case 11:
                return equip.LeftRing;
            case 12:
                return equip.Neck;
            case 13:
                return equip.RightHand;
            case 14:
                return equip.LeftHand;
            default:
                return 0;
        }
    }

    public static void ChangeEquip(Item item, int slot, Equip equip)
    {
        int it = (int)item.ItemType;
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<Item>(INVENTORY);
        Item itemOld;
        switch (slot)
        {
            case 1:
                if (it != 1)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Head != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Head)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Head = item.Id;
                    col.Update(item);
                }
                break;
            case 2:
                if (it != 2)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Body != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Body)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Body = item.Id;
                    col.Update(item);
                }
                break;
            case 3:
                if (it != 3)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Shoulder != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Shoulder)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Shoulder = item.Id;
                    col.Update(item);
                }
                break;
            case 4:
                if (it != 4)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Wrist != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Wrist)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Wrist = item.Id;
                    col.Update(item);
                }
                break;
            case 5:
                if (it != 5)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Hand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Hand)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Hand = item.Id;
                    col.Update(item);
                }
                break;
            case 6:
                if (it != 6)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Legs != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Legs)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Legs = item.Id;
                    col.Update(item);
                }
                break;
            case 7:
                if (it != 7)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Feet != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Feet)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Feet = item.Id;
                    col.Update(item);
                }
                break;
            case 8:
                if (it != 8)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Cape != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Cape)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Cape = item.Id;
                    col.Update(item);
                }
                break;
            case 9:
                if (it != 9)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Belt != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Belt)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Belt = item.Id;
                    col.Update(item);
                }
                break;
            case 10:
                if (it != 11)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.RightRing != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.RightRing)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.RightRing = item.Id;
                    col.Update(item);
                }
                break;
            case 11:
                if (it != 11)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.LeftRing != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.LeftRing)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.LeftRing = item.Id;
                    col.Update(item);
                }
                break;
            case 12:
                if (it != 12)
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else
                {
                    if (equip.Neck != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.Neck)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.Neck = item.Id;
                    col.Update(item);
                }
                break;
            case 13:
                if (!new[] { 21, 22, 23, 33, 41 }.Contains(it))
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else if (new[] { 24, 25, 26, 27, 28, 31, 32, 34, 43 }.Contains(it))
                {
                    if (equip.LeftHand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.LeftHand)).First();
                        itemOld.Qtd += 1;
                        equip.LeftHand = 0;
                        col.Update(itemOld);
                    }
                    if (equip.RightHand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.RightHand)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.RightHand = item.Id;
                    col.Update(item);
                }
                else
                {
                    if (equip.RightHand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.RightHand)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.RightHand = item.Id;
                    col.Update(item);
                }
                break;
            case 14:
                if (!new[] { 13, 21, 22, 23, 33, 41, 42 }.Contains(it))
                    Console.WriteLine("Este item não pode ser colocado nesse espaço");
                else if (new[] { 24, 25, 26, 27, 28, 31, 32, 34, 43 }.Contains(it))
                {
                    if (equip.LeftHand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.LeftHand)).First();
                        itemOld.Qtd += 1;
                        equip.LeftHand = 0;
                        col.Update(itemOld);
                    }
                    if (equip.RightHand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.RightHand)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.RightHand = item.Id;
                    col.Update(item);
                }
                else
                {
                    if (equip.LeftHand != 0)
                    {
                        itemOld = col.Query().Where(i => i.Id.Equals(equip.LeftHand)).First();
                        itemOld.Qtd += 1;
                        col.Update(itemOld);
                    }
                    item.Qtd -= 1;
                    equip.LeftHand = item.Id;
                    col.Update(item);
                }
                break;
        }
        db.Dispose();
    }
    public static void RemoveEquip(int slot, Equip equip)
    {
        var db = new LiteDatabase("data.db");
        var col = db.GetCollection<Item>(INVENTORY);
        Item itemOld;
        try
        {

            switch (slot)
            {
                case 1:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Head)).First();
                    itemOld.Qtd += 1;
                    equip.Head = 0;
                    col.Update(itemOld);
                    break;
                case 2:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Body)).First();
                    itemOld.Qtd += 1;
                    equip.Body = 0;
                    col.Update(itemOld);
                    break;
                case 3:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Shoulder)).First();
                    itemOld.Qtd += 1;
                    equip.Shoulder = 0;
                    col.Update(itemOld);
                    break;
                case 4:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Wrist)).First();
                    itemOld.Qtd += 1;
                    equip.Wrist = 0;
                    col.Update(itemOld);
                    break;
                case 5:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Hand)).First();
                    itemOld.Qtd += 1;
                    equip.Hand = 0;
                    col.Update(itemOld);
                    break;
                case 6:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Legs)).First();
                    itemOld.Qtd += 1;
                    equip.Legs = 0;
                    col.Update(itemOld);
                    break;
                case 7:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Feet)).First();
                    itemOld.Qtd += 1;
                    equip.Feet = 0;
                    col.Update(itemOld);
                    break;
                case 8:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Cape)).First();
                    itemOld.Qtd += 1;
                    equip.Cape = 0;
                    col.Update(itemOld);
                    break;
                case 9:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Belt)).First();
                    itemOld.Qtd += 1;
                    equip.Belt = 0;
                    col.Update(itemOld);
                    break;
                case 10:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.RightRing)).First();
                    itemOld.Qtd += 1;
                    equip.RightRing = 0;
                    col.Update(itemOld);
                    break;
                case 11:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.LeftRing)).First();
                    itemOld.Qtd += 1;
                    equip.LeftRing = 0;
                    col.Update(itemOld);
                    break;
                case 12:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.Neck)).First();
                    itemOld.Qtd += 1;
                    equip.Neck = 0;
                    col.Update(itemOld);
                    break;
                case 13:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.RightHand)).First();
                    itemOld.Qtd += 1;
                    equip.RightHand = 0;
                    col.Update(itemOld);
                    break;
                case 14:
                    itemOld = col.Query().Where(i => i.Id.Equals(equip.LeftHand)).First();
                    itemOld.Qtd += 1;
                    equip.LeftHand = 0;
                    col.Update(itemOld);
                    break;
            }
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Já não há itens equipados neste espaço.");
        }
        db.Dispose();
    }

    public static string DispayEquipment(InventoryController inventory, Equip equip, bool indexed = false)
    {
        string returnString = "Equipamento:";
        returnString += "\n\t" + (indexed ? "1: " : "") + "Cabeça: " + (equip.Head > 0 ? inventory.GetItem(equip.Head).Name : "vazio");
        returnString += "\n\t" + (indexed ? "2: " : "") + "Corpo: " + (equip.Body > 0 ? inventory.GetItem(equip.Body).Name : "vazio");
        returnString += "\n\t" + (indexed ? "3: " : "") + "Ombro: " + (equip.Shoulder > 0 ? inventory.GetItem(equip.Shoulder).Name : "vazio");
        returnString += "\n\t" + (indexed ? "4: " : "") + "Pulso: " + (equip.Wrist > 0 ? inventory.GetItem(equip.Wrist).Name : "vazio");
        returnString += "\n\t" + (indexed ? "5: " : "") + "Mãos: " + (equip.Hand > 0 ? inventory.GetItem(equip.Hand).Name : "vazio");
        returnString += "\n\t" + (indexed ? "6: " : "") + "Pernas: " + (equip.Legs > 0 ? inventory.GetItem(equip.Legs).Name : "vazio");
        returnString += "\n\t" + (indexed ? "7: " : "") + "Pés: " + (equip.Feet > 0 ? inventory.GetItem(equip.Feet).Name : "vazio");
        returnString += "\n\t" + (indexed ? "8: " : "") + "Costas: " + (equip.Cape > 0 ? inventory.GetItem(equip.Cape).Name : "vazio");
        returnString += "\n\t" + (indexed ? "9: " : "") + "Cintura: " + (equip.Belt > 0 ? inventory.GetItem(equip.Belt).Name : "vazio");
        returnString += "\n\t" + (indexed ? "10: " : "") + "Anel Direito: " + (equip.RightRing > 0 ? inventory.GetItem(equip.RightRing).Name : "vazio");
        returnString += "\n\t" + (indexed ? "11: " : "") + "Anel Esquerdo: " + (equip.LeftRing > 0 ? inventory.GetItem(equip.LeftRing).Name : "vazio");
        returnString += "\n\t" + (indexed ? "12: " : "") + "Pescoço: " + (equip.Neck > 0 ? inventory.GetItem(equip.Neck).Name : "vazio");
        returnString += "\n\t" + (indexed ? "13: " : "") + "Mão Direita: " + (equip.RightHand > 0 ? inventory.GetItem(equip.RightHand).Name : "vazio");
        returnString += "\n\t" + (indexed ? "14: " : "") + "Mão Esquerda: " + (equip.LeftHand > 0 ? inventory.GetItem(equip.LeftHand).Name : "vazio");
        return returnString;
    }
}