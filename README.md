# HWA SOCIAL

![HWA Social iOS](https://github.com/VB10/HWASocial/blob/master/demo/Screen%20Shot%202019-02-05%20at%2020.16.15.png?raw=true)

![HWA Social Androd](https://github.com/VB10/HWASocial/blob/master/demo/Screen%20Shot%202019-02-05%20at%2020.10.16.png?raw=true)

Öğrencilik döneminde gerçekleştirdiğimiz Xamarin forms projesinin open source çevirilmiş halidir.

> Proje canlıya çıkacak şekilde her hazırlanmıştır.Modüller çalışmakta eksikleri elbette bulunmakta amacım sizlere bir referans olması, değer katması ve yaşanmışlıkları diğer insanlara da aktarmak için geçirilmiştir.

----------
# Nedir ?
- Sosyal medya uygulamasıdır.
- Amaç kurum içi özel bir bağlantı ağı oluşturmaktır.
- İnsanlara daha fazla etkileşim sağlayarak onlara güncel haberleri aktarıp bunun yanı sıra onlardan feedback alındığı bir uygulamadır.
- Baştan sona Xamarin.Forms ile geliştirilmiştir.
- Uygulama backend servis olarak [firebase](https://console.firebase.google.com/u/0/) kullanmıştır
- Push Notification servisi olarak [OneSignal](https://onesignal.com/) kullanılmıştır.
- Crash reporting için [hockey app](https://hockeyapp.net/) tercih edilmiştir.
- Uygulama dağıtımı için demo sürecinde [hockey app](https://hockeyapp.net/) ve [apple test flight](https://developer.apple.com/testflight/) üzerinden sağlanmıştır.
-Uygulama da iconlar için sıklıkla [makeapp](https://makeappicon.com/) icon sitesi kullanıldı.

----------
# Gereklilikler
- Yerel(native) kod bilgisi (android java / ios swift)
- İyi derece c# bilgisi
- iOS ortam için mac cloud veya bir mac cihaz.
- Firebase db yapısı önceden kullanmış olma.

----------
# Teknik Detaylar
1. Api katmanında ki işlemler için [Repository Pattern](https://medium.com/@hardwareandro/geli%C5%9Ftiricinin-rehberi-55cf3e4703a3) kullanıldı.Singletton Pattern sayfa içerisinde ki user regNumber gibi  saklama işlerinde kullanıldı.
2.  Android için yaklaşık 6 yerel(native) sınıf yazıldı.Bu kodlar tabbar özelleştirmeden drawer componentine veya textfield'a kadar kullanıldı veya denendi.
3. IOS taraf içinde aynı sayı geçerli olup burada swift aracılığıyla özel işlemler yapıldı.Tab butona basılınca en başa dönme veya navbar left icon gibi xamarin forms'un bize vermedi nitelikler buradan eklendi.
4. Backend de [firebase rest database](https://firebase.google.com/docs/reference/rest/database/) ile real time db async işlemler ile sayfalama belirli aralıklı dataları çekme veya tek bir data çekmek gibi işlemler buradan sağlanmıştır.
5. MVVM patterni üzerine gidilmiştir.(Model ViewModel View) ortalama tüm sayfalarda kullanılmıştır(bir kaç sayfada süreçten ötürü geçilmedi).MVVM ile view'lerimizin arkasını boş bırakarak tüm işi vm katmanında hallediyoruz.Burada kullandığımız Observable pattern ile data değişikliklerini ui bind ettim.
6. c# Uygulama boyunca 101den 404'e kadar uygulamanın genelinde her yerde kullandık zira tüm c# yeteneklerini kullanmaya çalıştık.
7. View lerimizi genelde grid üzerine oturttuk ve buradaki grid'in flex gibi çalışması özelliğini kullandık(eksik olan yerlerimiz var daha kolaylaştırılabilir sayfalar mevcut).
8. Web Client'tan gelen dataları html parse ederek aslında bir editörden gelen yapıyı custom bir componente çevirdik.
9. BaseEnum yapılarıyla MessagingCenter'leri ortaklaştırdık.


----------
# Nasıl Kullanabilrim ?

> Projede kullandığımız Xamarin Firebase package'ti eskimiş kendilerini .net core 2+ ile kullnılmıyor ondan dolayı kendiniz yazarken projedeki mantığı referans alarak alıp bu paketi kullanabilirsiniz [Firebase Database](https://www.nuget.org/packages/FirebaseDatabase.net/)

- Kendinize bir firebase hesabı açıp herhangi bir proje oluştur
- Ardından buradan json dosyasını indirip real time database içerisine eklemelisiniz(içeri aktar diyerek)
- Alttaki kodları real time database içerisindeki rules altına eklemelisiniz buradaki ruller db'den data çekerken muhakkak eklemeniz gerekiyor.
```json
{
  "rules": {
    ".read": "auth != null",
    ".write": "auth != null",
    "activities": {
      ".indexOn": [
        "endDate"
      ]
    },
    "users": {
      ".indexOn": [
        "regNumber"
      ]
    },
    "feedbacks": {
      ".indexOn": [
        "department"
      ]
    },
    "surveys": {
      ".indexOn": [
        "endDate",
        "startDate",
        "createdDate"
      ]
    },
    "announcements": {
      ".indexOn": [
        "createdDate",
        "category"
      ]
    },
        "competitions": {
      ".indexOn": [
        "endDate",
        "startDate",
        "createdDate"
      ]
    },
      "license" : {
        ".read": "auth !=  null",
      }
  } 
}
```
- User auth işlemleri için ise yapmanız gereken firebase altında uthentication altında oturum açma yöntemi altından eposta şifre belirlemelisiniz(kodda @hardwareandro.com prefix'i unutmayın)
- Onesignal hesabı oluşturup site üzerinden gerekli adımları izlerseniz size rahatlıkla entegrasyonu yapacaktır.
- Projenin bundle veya appid'lerini değiştirmelisiniz.
- Youtube üzerinden tüm videolara erişebilirsiniz detaylı anlatım orarada bulunmakta.

----------
# Dikkat Edilmesi Gerekenler
- Aman diyim sabaha proje çalışmaz laflarını çok duyacaksınız lütfen git kullanın.
- Projenizi derlerken her zaman için ios android klasörleri altındaki bin ve obj'leri temizleyin
- Projeniz platform spesific hatalar veya nuget package bazlı hatalar verebilir native koda hızlıca düşüp kendiniz kod girin    
- c# ile geliştirmek kolay diyip mobil yazacağım düşüncesini unutup c# ile pattern'ler kullanmaya çalışın proje başında temiz gidin temiz olsun
- Ekranları tasarlarken bir preview aramayın koddan alışın çok hızlanacaksınız yapısı oldukça kolay binding'leri doğru verin.
- Best practices c# özelinde okuyun android ios özelinde lifecyle iyi bilin.
- Emulator'de sürekli bir ekranda çalışın misal diyelim önce ios iphone 7 göre bitirin sonra androide bakın ui hataları hızlıca çözülüecektir.
- Platform özelinde kod yazmaya çok çalışmayın tasarımınızı ortaklaştırıp xamarin'nin size sunduğu componentleri esas alın.
- Tasarım yaparken zeplin kullanın zeplinin extensionları ile tasarım yaparken kolay yöntemler bulablirsiniz.
- UpLabs'i eviniz gibi bilin
- Ben eleştiriye açık birisiyim sizde olun ve indandığınız şeyde yok xamarin kötü yok şu kötüden ziyade önce en iyi kodu yazmaya çalışın unutmayın kiii;

> ** Bilgisayarın anlayacağı kod yazmak kolaydır ama insanın anlayacağı kodu yazmak emek bilgi ve eleştiri gerektirir ** 

----------
# Teşekkürler
- Sayın İhsan hocama ayrı bir borcum bulunuyor emekleri ve inancı için teşekkür ederim.
- Projeye dahil olup gerek bırakan gerek inanmayan gereken para peşinde koşan tüm arkadaşlarıma özel teşekkür ederim    
- Projeyi bizlere getiren değerli şirket I.K sına teşekkür ederim.
- c# a teşekkür ederim xamarin'in konusunda binlerce anlatıp yapıp hiçbir emsal projesi olmayan hocalarımıza büyüklerimize teşekkür ederim.
- Makeapp icon da çok hayat kurtardı onada teşşekkür borç bilirim
- MacBook'uma büyük bir kalp atıyorum seviliyorsun.



> # Son olarak...
> Proje yaklaşık 6 ay süreçlerle uğraşa uğraşa 2 ayda biticek iş 1 yıl sürdü.
> Analiz yok bir ürün isteniyor fakat yazılım mimarı yok proje random başlıyor.
> Bu işler böyle olmaz olursa tek zarar gören siz olursunuz ve muhtemelen kimse size inanmaz.
> Yola çıkarken yanınıza birilerini almak konusunda bir çok kez düşünün ve bence almayın zira tek olup yapamamayı yola çıkıp satılmaya tercih ederim
> 1 yıl tecrübeli hiç yazmamış adamlar koskoca frameworklere dillere kötü diyebiliyor ondan çok aldanmamak lazım.
> Proje süresince o kadar çok arkadaşımı kaybettim ki hep kötü olan biz olduk.Proje'ye ben isteyerek  canlı'ya çıkmadan tek kuruş alınmayacak diye madde eklettirmiştim belkide bundandır.
> Neyse ben mükemmel değilim ama sadece şuan emek verip çalışan her insan mükemmel olacaktır ben inanıyorum ve hatalarımla eksiklerimle bu projeyi tüm dünyaya açıyorum umarım birilerinin işine yarar bir kaç güzel insan benim gibi günlerce zorluk çekmek yerine kafalarında bir ışık yanar ve çözerler 


Ben deniz  @Veli Bacik Saygılarım ve Sevgilerimle.. 2017-2019
- [Github](https://github.com/VB10)
- [Mail](hardwareandro@gmail.com)
- [Twitter](https://twitter.com/10VBacik)
- [Youtube](https://www.youtube.com/channel/UCdUaAKTLJrPZFStzEJnpQAg)






