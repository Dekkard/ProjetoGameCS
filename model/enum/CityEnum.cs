public enum CityName1 // Primeiro conjunto de sílabas
{
    Vin, Vi, Lan, Ian, Ter, Tir, Tyr, Kra, Kar, Yha, Nin, Gno, Hu, Ret, Sal, Ghul, Gull, Yelo,
}
public enum CityName2 // Segundo conjunto de sílabas
{
    fil, phil, phill, rut, rutt, hut, hutt, ter, ol, oll, rum, hull, opt, op, who, xafe, kha
}
public enum CityName3 // Terceiro conjunto de sílabas
{
    sur, kril, krill, suros, um, ksi, kara, ygo, kji, who, yal, yala
}
public enum CityName4 // Último conjunto de sílabas
{
    ka, bo, tum, gum, olm, duo, li, ma, kle, de, ho, qo, gi, to, si, ne, vier, au, ag,
}
public enum CitySuffix // Sufixo para as cidades
{
    burg, town, _City,
}
public enum CitySize // Tamanho da cidade
{
    Burg, Town, City, Capital, Polis
}
public enum TamanhoCidade // Tamanho da cidade traduzido
{
    Burgo, Vila, Cidade, Capital, Polis
}
public enum CityWorks // Tipos de trabalho que pode ter em uma cidade
{
    Minerador, Ferreiro, Tecelão, Alfaiate, Herbalista, Alquimista, Caçador, Coureiro, Ourives, Talhadeiro, Madereiro, Catalizante,
}
// Trabalho está relacionado aos atributos do herói, com isso, quando um trabalho é bem executado um ponto é adicionado ao atributo relacionado. Dependendo da complexidade do trabalho, mais de um atributo é involvido.
// Mineirador: Força e Resistência
// Ferreiro: Força, Resistência e Percepção
// Tecelão: Agilidade, percepção
// Alfaiate: Agilidade, percepção e inteligência
// Herbalista: Inteligência e Percepção
// Alquimista: Inteligência, agilidade, percepção
// Caçador: Agilidade e Percepção, Resistência
// Coureiro: Resistência, Percepção
// Orives: Percepção, agilidade e inteligência
// Talheiro: Percepção, 
// Madeireiro: Força e Resistência
// Catalizante: Inteligência, percepção, 
public enum CityServices // Tipos de serviço que uma cidade pode oferer
{
    Estaleiro, Loja, Mercearia, Armeiro, Alquimista, Arqueiro, Catalizadores,
}