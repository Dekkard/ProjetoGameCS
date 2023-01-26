using static BaseService;
using LiteDB;
#pragma warning disable 8600, 8604, 8625, 8603
public class EquipController
{

    LiteDatabase _db;
    private Equip _equip;

    public EquipController(LiteDatabase db, Equip equip)
    {
        _db = db;
        _equip = equip;
    }
    public int GetEquipSlot(int slot)
    {
        switch (slot)
        {
            case 1:
                return _equip.Head;
            case 2:
                return _equip.Body;
            case 3:
                return _equip.Shoulder;
            case 4:
                return _equip.Wrist;
            case 5:
                return _equip.Hand;
            case 6:
                return _equip.Legs;
            case 7:
                return _equip.Feet;
            case 8:
                return _equip.Cape;
            case 9:
                return _equip.Belt;
            case 10:
                return _equip.RightRing;
            case 11:
                return _equip.LeftRing;
            case 12:
                return _equip.Neck;
            case 13:
                return _equip.RightHand;
            case 14:
                return _equip.LeftHand;
            default:
                return 0;
        }
    }
    public void SetEquipSlot(Item item, int slot)
    {
        int itemId = 0;
        if (item != null)
        {
            item.Qtd -= 1;
            _db.GetCollection<Item>(INVENTORY).Update(item);
            _equip.AddTotalMod(item);
            itemId = item.Id;
        }
        switch (slot)
        {
            case 1:
                _equip.Head = itemId;
                break;
            case 2:
                _equip.Body = itemId;
                break;
            case 3:
                _equip.Shoulder = itemId;
                break;
            case 4:
                _equip.Wrist = itemId;
                break;
            case 5:
                _equip.Hand = itemId;
                break;
            case 6:
                _equip.Legs = itemId;
                break;
            case 7:
                _equip.Feet = itemId;
                break;
            case 8:
                _equip.Cape = itemId;
                break;
            case 9:
                _equip.Belt = itemId;
                break;
            case 10:
                _equip.RightRing = itemId;
                break;
            case 11:
                _equip.LeftRing = itemId;
                break;
            case 12:
                _equip.Neck = itemId;
                break;
            case 13:
                _equip.RightHand = itemId;
                break;
            case 14:
                _equip.LeftHand = itemId;
                break;
        }
    }
    public int[] GetSlotType(int slot)
    {
        switch (slot)
        {
            case 1: //HeadSlot
                return new[] { 1 };
            case 2: //BodySlot
                return new[] { 2 };
            case 3: //ShoulderSlot
                return new[] { 3 };
            case 4: //WristSlot
                return new[] { 4 };
            case 5: //HandSlot
                return new[] { 5 };
            case 6: //LegsSlot
                return new[] { 6 };
            case 7: //FeetSlot
                return new[] { 7 };
            case 8: //CapeSlot
                return new[] { 8 };
            case 9: //BeltSlot
                return new[] { 9 };
            case 10:
            case 11: //RingSlot
                return new[] { 11 };
            case 12: //NeckSlot
                return new[] { 12 };
            case 13: //MainHandSlot
                return new[] { 21, 22, 23, 33, 41, 42, 24, 25, 26, 27, 28, 31, 32, 34, 43 };
            case 14: //OffHandSlot
                return new[] { 13, 21, 22, 23, 33, 41, 42, 24, 25, 26, 27, 28, 31, 32, 34, 43 };
            case 15: //TwoHanded
                return new[] { 24, 25, 26, 27, 28, 31, 32, 34, 43 };
            default:
                return new int[] {};
        }
    }

    public void ChangeEquip(Item item, int slot)
    {
        int type = (int)item.ItemType;

        if (!GetSlotType(slot).Contains(type))
            return;

        if (GetSlotType(15).Contains(type))
        {
            RemoveEquip(13);
            RemoveEquip(14);
            SetEquipSlot(item, 13);
        }
        else
        {
            RemoveEquip(slot);
            SetEquipSlot(item, slot);
        }
    }
    public void RemoveEquip(int slot)
    {

        Item item = _getItem(GetEquipSlot(slot));
        if (item == null)
            return;
        item.Qtd += 1;
        SetEquipSlot(null, slot);
        _equip.ReduceTotalMod(item);
        _db.GetCollection<Item>(INVENTORY).Update(item);
    }
    public string DispayEquipment(bool indexed = false)
    {
        string returnString = "Equipamento:";
        returnString += "\n\t" + (indexed ? "1: " : "") + "Cabeça: " + (_equip.Head != 0 ? _getItem(_equip.Head).Name : "vazio");
        returnString += "\n\t" + (indexed ? "2: " : "") + "Corpo: " + (_equip.Body != 0 ? _getItem(_equip.Body).Name : "vazio");
        returnString += "\n\t" + (indexed ? "3: " : "") + "Ombro: " + (_equip.Shoulder != 0 ? _getItem(_equip.Shoulder).Name : "vazio");
        returnString += "\n\t" + (indexed ? "4: " : "") + "Pulso: " + (_equip.Wrist != 0 ? _getItem(_equip.Wrist).Name : "vazio");
        returnString += "\n\t" + (indexed ? "5: " : "") + "Mãos: " + (_equip.Hand != 0 ? _getItem(_equip.Hand).Name : "vazio");
        returnString += "\n\t" + (indexed ? "6: " : "") + "Pernas: " + (_equip.Legs != 0 ? _getItem(_equip.Legs).Name : "vazio");
        returnString += "\n\t" + (indexed ? "7: " : "") + "Pés: " + (_equip.Feet != 0 ? _getItem(_equip.Feet).Name : "vazio");
        returnString += "\n\t" + (indexed ? "8: " : "") + "Costas: " + (_equip.Cape != 0 ? _getItem(_equip.Cape).Name : "vazio");
        returnString += "\n\t" + (indexed ? "9: " : "") + "Cintura: " + (_equip.Belt != 0 ? _getItem(_equip.Belt).Name : "vazio");
        returnString += "\n\t" + (indexed ? "10: " : "") + "Anel Direito: " + (_equip.RightRing != 0 ? _getItem(_equip.RightRing).Name : "vazio");
        returnString += "\n\t" + (indexed ? "11: " : "") + "Anel Esquerdo: " + (_equip.LeftRing != 0 ? _getItem(_equip.LeftRing).Name : "vazio");
        returnString += "\n\t" + (indexed ? "12: " : "") + "Pescoço: " + (_equip.Neck != 0 ? _getItem(_equip.Neck).Name : "vazio");
        returnString += "\n\t" + (indexed ? "13: " : "") + "Mão Direita: " + (_equip.RightHand != 0 ? _getItem(_equip.RightHand).Name : "vazio");
        returnString += "\n\t" + (indexed ? "14: " : "") + "Mão Esquerda: " + (_equip.LeftHand != 0 ? _getItem(_equip.LeftHand).Name : "vazio");
        return returnString;
    }

    private Item _getItem(int itemId)
    {
        try
        {
            return _db.GetCollection<Item>(INVENTORY).Query().Where(i => i.Id == itemId).First();
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}