
# Kisdoktori generátor

## Feladat [2-3 mondat]

Egy olyan alkalmazás készítése, mely törzsanyagokból disszertációt állít elő. Törzsanyagokat lehet felvinni a rendszerbe, egy szerkesztőfelületen megadni a generálás paramétereit, majd a generált dokumentumot különböző formátumokba menteni.

## A kisháziban elérhető funkciók [adatmódosítással járó is legyen benne]
- törzsanyagok feltöltése (csak .txt formátumban), törlése - a törzsanyag témája megadható.
- egyszerű szerkesztőfelület - téma megadása, elvárt szószám
- egyszerű generálás - a téma alapján szövegek összeválogatása véletlenszerűen
- mentés XML formátumban, az egyes szövegrészekhez megadva, hogy melyik forrásműből származnak
- az előbbi XML betöltése és tartalmának megjelenítése (az egyes szövegrészekhez jelenjen meg, hogy melyik forrásműből való)

## Adatbázis entitások [min. 3 db.]
- törzsanyag
- dolgozat
- forráshivatkozás

## Alkalmazott alaptechnológiák [a szerver oldal mindenkinek ugyanez lesz, kliensoldal választható]
- adatelérés: Entity Framework Core v6
- kommunikáció, szerveroldal: ASP.NET Core v6
- kliensoldal: Postman

## Továbbfejlesztési tervek [opcionális, a pontrendszerből érdemes válogatni]
- hosztolás Azure-ban
- HiLo elsődleges kulcs alkalmazása
- logikai törlés (soft delete) globális szűrőkkel
- OData szolgáltatás megvalósítása
