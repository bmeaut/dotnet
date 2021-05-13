## Pontszerzési szabályok

* Egy jogcímen csak egyszer szerezhető pont (pl. nem lehet 3 külső osztálykönyvtárral 21 pontot összeszedni), kivéve ahol ezt külön jelezzük
* Részpontszám nem adható, kivéve, ahol intervallum van megadva
* Kliensoldali megoldásért nem adható pont
* A szoftvernek egységes funkcióhalmazt kell nyújtania, különálló, egymáshoz nem kapcsolódó funkciókból álló szoftver nem elfogadható. Azaz különálló tutorialok összefércelését nem díjazzuk.

## Véglegesítve 2021. tavaszi félévre (2021.04.22.)!

Véglegesítés után csak a következő típusú változások lehetnek
  * hallgatóknak kedvező változások (pl. új jogcímek)
  * elírások, megfogalmazásbeli pontosítások javítása
  * ellentmondások feloldása
  
Változások: lásd git history

## Társadalmi munka
* a véglegesített pontrendszer vagy gyakorlatjegyzet javítása, bővítése, módosítása pull request-tel **\[0-2, max. 5\]**
    * Helyesírási hiba is lehet, de az oktatók döntenek, hogy hány pontot (0-2) ér a módosítás
    * Többször is megszerezhető!
    * A gyakorlatjegyzet repo-ja: https://github.com/bmeaut/aspnetcorebook

## ASP.NET Core Web API
*  [HATEOAS](https://en.wikipedia.org/wiki/HATEOAS) linkek generálása a válaszban **\[7\]**
*  Web API Core által alapból nem támogatott HTTP ige (verb) implementálása **\[5-7\]**
   * pl. GET-hez hasonló működés **5**
   * pl. PATCH ige részleges módosításhoz **7**
   * pl. OPTIONS ige az erőforrás által támogatott igék lekérdezéséhez **7**
* verziókezelt API. Szemléltetés két különböző verziós API egyidejű kiszolgálásával. **\[7-10\]**
   * nem HTTP header (pl. URL szegmens) alapján **7**
   * HTTP header alapján **10**
* API (egy részének) védése felhasználó által igényelhető API kulccsal **\[7\]**
* cache megvalósítása E-TAG használatával **\[3-8\]**
  * a kliens felüküldi az E-TAG-et, szerver összeveti az adatbázisból felolvasott verzióval **3**
  * a szerver is cache-ből olvassa ki az aktuális verziót **+5**
* Szerver oldali autentikáció. Saját token provider készítése, használata esetén nem jár pont. **\[7-15\]**
  * ASP.NET Core Identity middleware-rel, süti alapú - csak böngészős/Postman kliens esetén! **7**
  * token alapú, ASP.NET Core Identity + IdentityServer4/IdentityServer5/OpenIddict middleware-rel, nem-interaktív flow (pl. ROPG) **10**
  * token alapú, ASP.NET Core Identity + IdentityServer4/IdentityServer5/OpenIddict middleware-rel, interaktív flow **12** TODO: angular-osoknak túl könnyű
  * Azure AD B2C-re (ingyenes szint) építve **10**
  * más Identity-as-a-Service szolgáltatással (pl. Auth0) **7**
  * legalább egy külső identity provider integrálása (Google login, Windows login, stb.)  **+3**
* szerver oldali hozzáférés-szabályozás, az előbbi authentikációra építve  **\[2-5\]**
    * szerepkör alapú hozzáférés-szabályozás **2**
    * claim alapú hozzáférés-szabályozás **5**
* külső online szolgáltatás (Twitter, Facebook, Google Maps, Bing Maps, stb.) integrálása a szerveroldali alkalmazásba klienskönyvtárral (pl. HttpClient) vagy SDK-val **\[7-10\]**
  * egyszerű REST API, SDK használat nélkül, egyszerű API kulcs alapú authentikáció **7**
  * SDK-val / REST API-val, authentikációt (pl. OIDC) végrehajtva **10**
* SignalR Core alkalmazása valós idejű, szerver felől érkező push jellegű kommunikációra **\[7\]**
* hosztolás külső szolgáltatónál **\[5-7\]**
  * Azure (ingyenes [App Services - WebApp szolgáltatás](https://docs.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore)) **7**
  * egyéb szolgáltató **5**
* hosztolás service-ben (Windows Service, Linux systemd) **\[7-10\]**
  * Windows service **7**
  * Linux systemd **10**
* Publikálás docker konténerbe és futtatás konténerből **\[7\]**
* OpenAPI leíró (swagger) alapú dokumentáció **\[3-5\]**
  * minden végpont kliens szempontjából releváns működése dokumentált, minden lehetséges válaszkóddal együtt **3**
  * az API-nak egyidejűleg több támogatott verziója van, mindegyik dokumentált és mindegyik támogatott verzió dokumentációja elérhető  **+2**
* ~~WebHook-ok használata külső szolgáltatással (pl. github, slack) **\[7\]**~~  **(egyelőre nincs hivatalos támogatás, csak Lab projekt)**

## Kommunikáció, hálózatkezelés
* alacsony szintű kommunikáció (soros port, HTTP alatti OSI réteg, pl. kétirányú TCP) **\[10\]**
* HTTPS kommunikáció (self-signed tanúsítvánnyal) az ASP.NET Web API és a kliens között, hosztolás normál, nem fejlesztői webszerverben (pl. Kestrel, Apache, nginx, nem IIS Express), szemléltetés Fiddler-rel **\[3-12\]**
  * csak szerver oldali tanúsítvány Kestrel-en **3**
  * csak szerver oldali tanúsítvány nem Kestrel-en (Apache, nginx, stb.) **7**
  * kliens is azonosítja magát tanúsítvánnyal a szerver felé **+5**
* az API funkciók egy részének elérhetővé tétele GraphQL hívásokon keresztül, ASP.NET Core middleware segítségével (pl. [GraphQL.NET](https://graphql-dotnet.github.io/) vagy [Hot Chocolate](https://chillicream.com/docs/hotchocolate)) az EF entitásmodellre építve. zemléltetés példahívásokkal a kliensből. **\[7-10\]**
  * csak lekérdezés **7**
  * módosítás vagy hozzáadás is (mutáció) **+3**
* az API funkciók egy részének elérhetővé tétele gRPC HTTP/2 vagy gRPC-Web hívásokon keresztül. Szemléltetés példahívásokkal kliensből vagy gRPC teszteszközből (pl. [bloomrpc](https://github.com/uw-labs/bloomrpc)) ***Azure App Service-szel, IIS-sel, böngészős klienssel korlátozottan [kompatibilis](https://docs.microsoft.com/en-us/aspnet/core/grpc/supported-platforms)!*** **\[7\]**
* az EF adatmodell kiajánlása OData szolgáltatás segítségével (*Microsoft.AspNetCore.OData* csomag). Példahívás bemutatása a kliensből OData v4 protokollt használva.  **\[7-10\]**
  * csak lekérdezés **7**
  * módosítás vagy hozzáadás vagy törlés is **+3**


## Entity Framework Core
* leszármazási hierarchia leképezése Entity Framework-kel (legalább kétszintű, legalább 3 tagú hierarchia) **\[3-7\]**
  * TPH, a diszkriminátor mező testreszabásával (saját mezőnév vagy saját értékek) **3**
  * TPT-vel **5**
  * ~~TPC-vel **7**~~ **(EF Core jelenleg nem támogatja)**
* MS SQL/LocalDB-től eltérő adatbáziskiszolgáló használata EF Core-ral (kivéve sqlite) **\[10-12\]** TODO: Azure SQL
  * Azure Cosmos DB **12**
  * egyéb, EF Core v5 támogatott adatbázis **10**  
* ~~saját Code-First konvenció készítése **\[5\]**~~  **(EF Core jelenleg nem támogatja)**
* saját szabályszerűség (konvenció) implementálása vagy meglevő felülbírálása reflexióval és/vagy Model API-val **\[5\]**
* saját többesszámosító (`IPluralizer`) - nem kell nyelvtanilag helyesnek lennie **\[7\]**
* automatikus újrapróbálkozás beállítása tranziens adatbázishibák (pl. connection timeout) ellen **\[2\]**
* Table splitting **\[5\]**
* ~~Entity splitting **\[5\]**~~  **(EF Core jelenleg nem támogatja)**
* alternatív kulcs **\[3-5\]**
  * alternatív kulcs bevezetése valamelyik entitásban **3**      
  * más entitás kapcsolattal hivatkozik az alternatív kulcsra **+2**
* adatbázis index konfigurációja az EF modellben **\[3\]**
* HiLo elsődleges kulcs alkalmazása **\[7\]**
* birtokolt típus (owned type) használata **\[3\]**
* adatbetöltés (seeding) migráció segítségével (`HasData`) **\[3\]**
* értékkonverter (value converter) alkalmazása EF Core leképezésben **\[3-5\]**
  * beépített, vagy külső komponensből származó value converter **3**
  * saját value converter **5**
  
## .NET Core részfunkciók alkalmazása
* az EF Core működőképességét jelző health check végpont publikálása a *Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore* NuGet csomag használatával **\[3\]**
* kifejezésfa (ExpressionTree) értelmezése/ksézítése/módosítása az [Expression API](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/#creating-expression-trees-by-using-the-api) használatával **\[5-20\]**
    * pl. szűrés dinamikusan, paraméterből érkező property neve alapján (pl. `o => o.Prop == propNev`) **5**
    * pl. keresés kapcsolódó kollekcióban dinamikusan (pl. `o => o.Coll.Any(e => e.Prop == propNev)`) **10**
    * saját LINQ provider - **előzetes egyeztetés szükséges!** **20**
* explicit kölcsönös kizárás helyett _ConcurretBag/ConcurrentQueue/ConcurrentStack/ConcurrentDictionary_ használata olyan rétegben, ahol párhuzamos hozzáférés valóban előfordul **\[5\]**
* lock-free algoritmus implementálása és használata (könyvtári implementáció felhasználása nélkül, `Interlocked` függvények használatával) **\[10\]**
* unit tesztek készítése  **\[7-14\]**
  * minimum 10 függvényhez **7**
  * a unit tesztekben a mock objektumok injektálása **+3**
  * EF Core memória-adatbázis vagy sqlite (vagy in-memory sqlite) használata teszteléshez **+4**
* XML validálás, alkalmazkodás meglévő XML formátumhoz pl. publikus webes sémához (RSS, opml) **\[7\]**
* Optimista konkurenciakezelés **\[5-15\]**
  * ütközésdetektálás és automatikus ütközésfeloldás **5**
  * ütközésfeloldás a felhasználó döntése alapján: _client wins_ vagy _store wins_ feloldással. Ütközés esetén a felhasználótól megkérdezzük, hogy a két adatverzió közül melyik legyen mentve az adatbázisba: az aktuális felhasználóé, a másik felhasználóé. Bemutatáskor szemléltetés egy példán keresztül. **10**
  * a felhasználó az eredeti értéket is választhatja (a módosítások előtti érték visszaállítása) **+5**
* pesszimista konkurenciakezelés (adatbázisobjektumok zárolása) bizonyos entitások/funkciók esetén, nem kell a teljes alkalmazásban alkalmazni. Bemutatáskor szemléltetés egy példán keresztül. **\[10\]**
* diagnosztika beépített vagy külső komponens segítségével **\[5-9\]**
  * legalább két célba, amiből legalább egy perzisztens (pl. fájl vagy adatbázis vagy külső szolgáltatás) **5**
  * struktúrált naplózás (structured logging) **+2**
  * fájl cél esetén rolling log (minden napon/héten/10 MB-onként új naplófájl) **+2**
  * az egyik cél egy külső naplózó szolgáltatás (pl. Azure Application Insights) **+2**
* áthívás nem felügyelt környezetbe (pl. natív Win32, natív linux) **\[7 - 12\]**
    * legalább egy nem egyszerű típus átadása/átvétele paraméterként **7**
    * saját natív kód használata, összetett típus átadásával **12**
* külső komponens használata DTO-k inicializálására **\[3\]**
   * Object mapper, pl. [AutoMapper](http://automapper.org/), [QueryMutator](https://github.com/yugabe/QueryMutator) **3**
   * Explicit kódgeneráló, pl. [MappingGenerator](https://github.com/cezarypiatek/MappingGenerator) **3**
* logikai törlés (soft delete) megvalósítása. A logikailag törölt elemek alapértelmezésben nem lekérdezhetőek - ezen szűrés megvalósítása globális szűrőkkel (Global Query Filter) **\[5\]**
* háttérművelet(ek) megvalósítása háttérfolyamat kezelő ASP.NET Core middleware komponenssel, pl. Quartz.NET, Hangfire **\[7\]**

## Kiegészítő, kapcsolódó technológiák alkalmazása
* [Rx.NET](https://github.com/dotnet/reactive) használata ([dokumentáció](http://reactivex.io/)) **\[7-10\]**
    * néhány alap Rx operátor használata **7**
    * két külső adatforrás integrálása **10**
* F# modul készítése és meghívása. Legalább az egyik legyen benne ezek közül: pattern matching, async, magasabb rendű függvény **\[7\]**
* külső osztálykönyvtár használata szerver oldalon. A külső komponens által megvalósított funkcionalitásért, képességért további pontszám nem adható. Nem számít ide a projekt generálásakor automatikusan bekerülő, illetve a Microsoft által készített, az alaptechnológiák függőségeit jelentő NuGet csomagok **\[7\]**
* platformfüggetlen kódbázisú szerveralkalmazás készítése és bemutatása legalább 2 operációs rendszeren az alábbiak közül: Windows, Linux, Mac, ARM alapú OS (Raspberry Pi). **\[7\]**

## Konkrét funkciók
* NET Compiler platform (Roslyn) Diagnostic Analyzer **\[3-7\]**
  * egyszerű analyzer, pl. property név konvenciók ellenőrzése **3**
  * bonyolultabb analyzer és kód fix is, pl. kiemelés metódusba **7**
