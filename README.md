# SOA

Glavna ideja sistema je da simulira praćenje meteoroloških parametara (temperatura, stepen oblačnosti, vazdušni pritisak, količina padavina, vlažnost vazduha, brzina vetra) i izvršavanje odgovarajuće komande na osnovu analize istih. Od tehnologija koriscen je .NET CORE za implementaciju mikroservisa i čist javascript za implementaciju grafickog interfejsa.

## Kratak opis mikroservisa

### Sensor Device Service

Device mikroservis čita podatke sa gore pomenutih 6 senzora i prosleđuje ih Data mikroservisu putem http post zahteva. Čitanje podataka sa senzora simulira se periodičnim 
čitanjem podaka iz fajla. Moguće je podešavanje parametara očitavanja u vidu promene graničnih vrednosti očitavanja.

### Sensor Data Service

