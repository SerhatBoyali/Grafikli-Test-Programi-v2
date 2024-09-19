# Grafikli Test Programı
Modbus Protokolü ile veri okuyup işleyen Test Uygulamasının güncel sürümü.
serhat.boyali@outlook.com mail adresi ile benimle iletişime geçebilirsiniz.

# Değişiklikler
- Bütün buglar hata hatalar düzeltildi
- Elektrik kesintisi, şalter atması ve benzeri durumlara karşı veri kaybını önlemek amaçlı değişiklik yapıldı.
- Veri okuma hızı eklendi.
- Veriyi Excell e yazdırma hızı eklendi.
- Aniden kapanmaya karşı koruma eklendi.
- Port bağlantısı veya USB Serialport dönüştürücü bağlantısının kopması durumunda otomatik veri kaydetme eklendi.

# Uygulamanın tanımı
Tasarladığımız test cihazını modbus protokolü kullanarak bilgisayara bir usb dönüştürücüsüyle bağlıyoruz. Cihazdan Voltaj, Akım ve NTC ısı verileri gelmektedir ve akım ile voltaj çarpımından WATT (güç) değerini ölçüyorum. Veriler 2 adet 8 er bit şeklinde gelmekte (1 veri 16 bit). Uygulamada bu iki veriyi 1 float verisine dönüştürüyorum. 10 tane cihaza kadar test edebilecek şekilde tasarladım. Cihazın ID sine göre tabpage ve ZedGraph kütüphanesini kullandığım grafik ekranım var. ZedGraph grafik ekranı grafiği kaydetme, yakınlaştırma, uzaklaştırma, yazdırma, ölçeği sıfırlama ve verilerin değerlerini gösterme işlemlerini kolayca yapmaktadır. Ölçülen verileri düzenli bir şekilde Excell dosyası haline getirir. Veri ölçme hızı, grafik ekranına işleme hızı ve excell listesine yazdırma hızı ayarlanabilir.

# Kullanım Talimalı
1. Modbus protokolü ile tasarladığınız test cihazınızı bir USB dönüştürücü ile bilgisayarınızı takınız, USB dönüştürücünün sürücüsünü indirmeyi unutmayınız. 
Uygulamayı çalıştırdıktan sonra port ve bautrate seçimi yapınız ardından Porta Bağlan butonuna basınız. Bağlantı başarılı olursa olumlu veya olumsuz mesaj gelecek ve "Port Bağlantısı:" durumu Açık veya Kapalı diye güncellenecek.
2. Port bağlantısı kurulduktan sonra test edeceğiniz cihazın ID numarasına göre Slave ID sayfası seçin.
3. Veriyi kaç saniyede bir ölçmek istediğinizi ve Excell sayfasına kaç saniyede bir yazdırmak istediğinizi "Saniye Belirleyiniz" Bölümüne giriniz. Bu TextBox a bir değer girmezseniz otomatik olarak 1 saniye olacaktır.
4. Connect diyerek bağlanın. Bağlantının başarılı yada başarısız olma durumu mesaj bildirimi ile gelecektir ve "Bağlantı Açık" veya "Bağlantı Kapalı" diye yazacaktır.
5. Veriler grafik ekranına ve textbox'larda görünmeye başlayacaktır.
6. Disconnect butonu ile grafiğe veri aktarma işlemini durdurabilirsiniz ve Bağlantıyı Kes butonu ile port bağlantısını durdurabilirsiniz.
7. Verileri Kaydet butonuna tıklayarak ölçtüğünüz verileri bir excell sayfası halinde kaydedebilirsiniz. Veriler "Sistem Saati Test Süresi VOLTAJ AKIM NTC1 NTC2 NTC3 NTC4 NTC5 NTC6 WATT" sütunlarında saniyede bir kaydedilir.
8. Grafik Ekranına Sağ tıklayıp "Farklı Kaydet" diyerek grafik görselini de kaydedebilirsiniz.
9. Formu temizle butonu ile ayarları sıfırlayabilirsiniz.
10. Programı kapat butonu veya formun sağ üst bölgesinde kapatma işaretine tıklayarak uygulamayı kapatabilirsiniz ama yanlışlıkla kapanmaya karşı uyarı olarak emin misiniz sorusu soran bir bildirimle karşılaşacaksınız.
11. USB Seri port dönüştürücüsünün çıkması gibi durumlarda program durur ve bildirim gönderir. USB yi tekrar çalışır durumda bilgisayara taktıktan sonra tekrar "Connect" butonuna basarsanız teste kaldığınız yerden devam edebilirsiniz.
12. Elektrik kesintisi, şalter atması, porttan veri okunamamasına sebep olacak her hangi bir etken durumunda program zaman aşımına girer ve kısa bir süre bekler, eğer veri gelmezse program portu güvenli bir şekilde kapatır ve veri kaybı yaşamamanız için test yapmış olduğunuz bütün verileri güvenli bir şekilde masaüstüne kaydeder. "Test_{Test ID si}_{Tarih-Saat}.xslx" formatında isimlendirilir.

13. ![image](https://github.com/user-attachments/assets/826f7388-e21b-4eca-a44e-ca1e0b5124c2)

14. ![Ekran görüntüsü 2024-07-19 232046](https://github.com/user-attachments/assets/24ad2e07-e9d0-4fad-ab71-f6346f5fbc69)

15. Excell e veri yazdırma hızını 3 saniye olarak belirlediğimizde gösterilecek formatın örneği ;

16. ![image](https://github.com/user-attachments/assets/c72a091a-626d-4155-bbda-85ed6be78b06)


