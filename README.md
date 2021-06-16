# SOA - Seminarski

Tema seminarskog: Nuclio - Serverless Platform for Automated Data Science.  
  
Svi potrebni detalji vezani za funkcionalnost, arhitekturu, kao i primeri funkcija koji se mogu izvršavati preko ove platforme dati su na prezentaciji.

# SOA - Projekat

Glavna ideja sistema je da simulira praćenje meteoroloških parametara (temperatura, stepen oblačnosti, vazdušni pritisak, količina padavina, vlažnost vazduha, brzina vetra) i izvršavanje odgovarajuće komande na osnovu analize istih. Od tehnologija koriscen je .NET CORE za implementaciju mikroservisa i čist javascript za implementaciju grafickog interfejsa.

## Arhitektura aplikacije i kratak opis mikroservisa

Aplikacija je realizovana tako da sadrži 6 mikroservisa.

### Sensor Device Service

Device mikroservis čita podatke sa gore pomenutih 6 senzora i prosleđuje ih Data mikroservisu putem http post zahteva. Čitanje podataka sa senzora simulira se periodičnim čitanjem podaka iz fajla. Moguće je podešavanje parametara očitavanja u vidu promene graničnih vrednosti očitavanja.

### Sensor Data Service

Data mikroservis prima podatke od Device mikroservisa, upisuje ih u bazu (mongoDB) i prosleđuje u Analytics mikroservis putem brokera (rabbitMQ).

### Sensor Analytics Service

Analytics mikroservis prima podatke sa Data mikroservisa, analizira ih u cilju detektovanja trenutnog stanja na osnovu parametara koji mogu uticati na isključivanje/uključivanje određenog senzora (npr. ako je oblačnost 0, nema smisla da bude uključen senzor za količinu padavina). Takođe ovaj mikroservis vrši upis podataka u bazu (influxDB) i šalje komandnu akciju Command mikroservisu putem brokera.

### Sensor Command Service

Command mikroservis dobija podatke od Analytics mikroservisa preko brokera i u zavisnoti akcije isključuje/uključuje odgovarajući senzor na Device mikroservisu. Prosležuje interfejsu aplikacije izvršenu akciju preko SignalR-a.

### Sensor Gateway Service

Gateway mikroservis predstavlja REST API za veb klijenta.

### Web dashboard

Web dashboard predstavlja grafički interfejs ove aplikacije, nudi prikaz podataka i važnih obaveštenja o trenutnim akcijama vezanim za senzore.

## Komunikacije putem brokera

Za slanje poruka web klijentu od strane command mikroservisa koristi se SignalR biblioteka za web socket komunikaciju. Za komunikaciju između Data, Analytics i Command mikroservisa koristi se rabbitMQ. U aplikaciji postoje dva topic-a, jedan za komunikaciju izmedju Data i Analytics mikroservisa i drugi za komunikaciju izmedju Analytics i Command mikroservisa.
