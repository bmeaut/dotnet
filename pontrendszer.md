## Pontszerzési szabályok

*   Egy jogcímen csak egyszer szerezhető pont (Pl.: nem lehet 3 külső osztálykönyvtárral 21 pontot összeszedni)
*   Részpontszám nem adható, kivéve, ahol intervallum van megadva

## **Még nincs véglegesítve 2018. tavaszi félévre!**

**Véglegesítés után csak hallgatóknak előnyös módosítások lehetnek (pl. új jogcímek).**

Utolsó módosítás: 2017.05.18. (csak véglegesítés)

## ASP.NET Core
*  teljes HATEOAS implementáció **\[10\]**
*  Web API által alapból nem támogatott HTTP ige implementálása **\[5-10\]**
   * pl. GET-hez hasonló működés **5**
   * pl. OPTIONS RFC2616 szerint **10**
* verziókezelt API **\[5-8\]**
   * HTTP header alapján **+3**
* API egy részének védése felhasználó által igényelhető API kulccsal **\[7\]**
* cache megvalósítása E-TAG használatával **\[3\]**
* ~~ODATA API használata a szerver-kliens kommunikációban **\[7\]**~~  ****(EF Core jelenleg nem támogatja)****
* Szerver oldali autentikáció **\[10-18\]**
* csak ASP.NET Identity **10**
* Identity facebook és/vagy Google, Microsoft Account támogatással **+3**
* Identity saját JWT token alapú autentikációval **+5**
* szerver oldali hozzáférés-szabályozás **\[2-6\]**
    * szerepkör alapú hozzáférés-szabályozás **2**
    * claim alapú hozzáférés-szabályozás **+4**
* külső online szolgáltatás (Twitter, Facebook, Google Maps, Bing Maps, stb.) integrálása a szerveroldali alkalmazásba klienskönyvtárral (pl. HttpClient) vagy SDK-val **\[7-10\]**
*   egyszerű REST API, SDK használat nélkül, egyszerű API kulcs alapú authentikáció **7**
*   SDK-val / REST API-val, authentikációt (pl. OAuth) végrehajtva **10**
*   SignalR alkalmazása valós idejű, szerver felől érkező push jellegű kommunikációra **\[7\]**
*   hosztolás külső szolgáltatónál **\[5-7\]**
*   Windows Azure (ingyenes App Services - WebApp szolgáltatás, de bankkártya regisztráció szükséges) **\[7\]**
*   egyéb szolgáltató **\[5\]**
* ~~WebHooks használata külső szolgáltatással (pl. github, slack) **\[7\]**~~  ******(EF Core jelenleg nem támogatja)******
## Kommunikáció, hálózatkezelés
* alacsony szintű kommunikáció (soros port,  HTTP alatt, pl. kétirányú TCP) **\[10\]**
* HTTPS kommunikáció (self-signed tanúsítvánnyal) az ASP.NET Web API és a kliens között, szemléltetés Fiddler-rel **\[7-12\]**
*  csak szerver oldali tanúsítvány, hosztolás IIS-ben (nem IIS Express) **7**
*  kliens is azonosítja magát tanúsítvánnyal a szerver felé **+5**

## Entity Framework Core
* leszármazási hierarchia leképezése Entity Framework-kel (legalább kétszintű, legalább 3 tagú hierarchia) **\[3-7\]**
  * TPH, a diszkriminátor mező testreszabásával (saját mezőnév vagy saját értékek) **3**
  * ~~TPT-vel **5**~~ **(EF Core jelenleg nem támogatja)**
  * ~~TPC-vel **7**~~ ****(EF Core jelenleg nem támogatja)****
*   MS SQL-től eltérő adatbáziskiszolgáló használata EF Core-ral (kivéve sqlite) **\[12\]**
*   ~~saját Code-First konvenció készítése **\[5\]**~~  ******(EF Core jelenleg nem támogatja)******
*   saját szabályszerűség (konvenció) implementálása vagy meglevő felülbírálása reflexióval és/vagy Model API-val **\[5\]**
*   saját többesszámosító (ScaffoldingModelFactory) - nem kell nyelvtanilag helyesnek lennie **\[7\]**
*   saját újrapróbálkozó (execution strategy) készítése és használata tranziens adatbázishibák (pl. connection timeout) ellen **\[7\]**
*   ~~Table splitting vagy entity splitting **\[5\]**~~  ******(EF Core jelenleg nem támogatja)******
*   alternatív kulcs **\[3-5\]**
*   alternatív kulcs bevezetése valamelyik entitásban **\[3\]**      
*   más entitás kapcsolattal hivatkozik az alternatív kulcsra **+2**
*   adatbázis index konfigurációja az EF modellben **\[3\]**
*   Entitások és adatbáziskontextus használata kizárólag a DAL rétegben (internal osztályok) **\[8\]**
*   HiLo elsődleges kulcs alkalmazása **\[7\]**
## .NET Core részfunkciók alkalmazása
*   kifejezésfa (ExpressionTree) értelmezése és manipulálása **\[5 - 20\]**
    *   pl. szűrés dinamikusan, paraméterből érkező property neve alapján (pl. o => o.Prop == x) **5**
    *   pl. keresés kapcsolódó kollekcióban dinamikusan (pl. o => o.Coll.Any(e => e.Prop == x)) **10**
    *   saját LINQ provider **20**
*   explicit kölcsönös kizárás helyett _ConcurretBag/ConcurrentQueue/ConcurrentStack/ConcurrentDictionary_ használata olyan rétegben, ahol párhuzamos hozzáférés valóban előfordul **\[5\]**
*   lock-free algoritmus implementálása és használata (könyvtári implementáció felhasználása nélkül, Interlocked függvények használatával) **\[10\]**
* unit tesztek készítése  **\[5-12\]**
  * minimum 10 függvényhez **5**
  * a unit tesztekben a mock objektumok injektálása **+3**
  * EF Core memória-adatbázis használata teszteléshez **+4**
*   XML validálás, alkalmazkodás meglévő XML formátumhoz pl. publikus webes sémához (RSS, opml) **\[7\]**
*   Optimista konkurenciakezelés **\[5-15\]**
*   ütközésdetektálás és automatikus ütközésfeloldás **5**
*   ütközésfeloldás a felhasználó döntése alapján: client wins vagy store wins feloldással. Ütközés esetén a felhasználótól megkérdezzük, hogy a két adatverzió közül melyik legyen mentve az adatbázisba: az aktuális felhasználóé, a másik felhasználóé. Bemutatáskor szemléltetés egy példán keresztül. **10**
*   a felhasználó az eredeti értéket is választhatja (a módosítások előtti érték visszaállítása) **+5**
*   pesszimista konkurenciakezelés (adatbázistáblák lock-olása) egy felületen. Bemutatáskor szemléltetés egy példán keresztül. **\[15\]**
*   diagnosztika beépített vagy külső komponens segítségével legalább két célba, amiből legalább egy perzisztens (pl. fájl vagy adatbázis) **\[5\]**
*   áthívás nem felügyelt környezetbe (pl. natív Win32, natív linux) **\[7 - 12\]**
    *   legalább egy nem egyszerű típus átadása/átvétele paraméterként **7**
    *   saját natív kód használata, összetett típus átadásával **12**
*    Object mapper (pl. [AutoMapper](http://automapper.org/), [QueryMutator](https://www.nuget.org/packages/QueryMutator/1.3.1)) használata DTO-k létrehozására **\[5\]**

## Kiegészítő, kapcsolódó technológiák alkalmazása
*   Beépülőkkel bővíthető alkalmazás készítése [MEF](http://msdn.microsoft.com/en-us/library/dd460648.aspx) használatával, legalább 1 pluginnel **\[10\]**
*   [Rx](http://msdn.microsoft.com/en-us/data/gg577609) Framework használata **\[7-10\]**
    *   néhány alap Rx operátor használata **7**
    *   két külső adatforrás integrálása **10**
*   F# modul készítése és meghívása. Legalább az egyik legyen benne ezek közül: pattern matching, async, magasabb rendű függvény **\[7\]**
*   külső osztálykönyvtár használata (a külső komponensért további pontszám nem adható). Nem számít ide a projekt generálásakor bekerülő, illetve a Microsoft által készített NuGet csomagok (pl. JSON.NET)**  \[7\]**
*   platformfüggetlen kódbázisú szerveralkalmazás készítése és bemutatása legalább 2 operációs rendszeren az alábbiak közül: Windows, Linux, Mac, ARM alapú OS (Raspberry Pi). **\[7\]**
## Konkrét funkciók
*   meglévő, magasabb szintű hálózati protokoll implementációja (pl. BitTorrent, GNUtella) **\[20\]**
*   NET Compiler platform (Roslyn) Diagnostic Analyzer **\[3-7\]**
*   egyszerű analyzer, pl. property név konvenciók ellenőrzése **3**
*   bonyolultabb analyzer és kód fix is, pl. kiemelés metódusba **7**
