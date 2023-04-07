1 => Yapýlan incelemelere göre ilgili projenin profesyonel olarak en hafif versiyonda yazýlmasý istendiðinden Proje N-Tier mimari paternine göre yazýlacaktýr...

2 => Projede acýlacak katmanlar
ENTITIES,
MAP,
DAL,
BLL,
COMMON (Ortak kullanýlacak sýnýflarýn tutulacagý katman olacaktýr),
API - UI (Projemizi dýsarý acmak istedigimiz icin actýgýmýz ayrý bir katmandýr)
DTO(API'daki esnekligimizin proje bazlý artmasý icin acagýmýz katmandýr)
UI(MVC olarak düsünülmüþtür)
VM(View Models katmaný)

3 => Projede degiþken isimleri degiþkenler local variable olduðu sürece camelCase, field olduðu sürece basýnda _sembolü olmak üzere camelCase; Property, metot ve class isimleri PascalCase standartlarýnda yazýlacaktýr ve projede Türkçe karakter kullanýlmayacaktýr...Class isimleri kesinlikle cogul olmayacaktýr...

4=> Katman isimleri Project ile baslayacak . sembolünden sonra katmansal takýlarla bitecektir (Project.ENTITIES vs...)

5=> Tasarým patternlerinden Generic Repository,Singleton Pattern  kesinlikle kullanýlacaktýr...Mevcut algoritmada bu tasarým  patternlerinin dýsýnda bir baska Tasarým Paterni öngörülmemektedir...Ancak projenin esnekligi buna elveriþli olduðu takdirde, projenin mimari paterniyle bir çakýþma olmamasý kaydýyla ( bu durum özel olarak istenecek olan bir Arge süreci icerisinde hemen belirlenmek zorundadýr) gerekli görülürse rahatca eklenebilecektir...

***************