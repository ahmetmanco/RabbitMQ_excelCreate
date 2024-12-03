RabbitMQ ile Veritabanı product tablosundaki verileri excel dosyası olarak indirme

Bu projede kullanılan dosya içinde ekli olan "Northwind" veritabanı örnek olarak alınmıştır. Veritabanı içindeki Product tablosu kullanılmıştır.

Basit kullanıcı güvenliği Identity ile oluşturulup gerekli kullanıcı bilgileri(Email ve şifre), SeedData içinde mevcuttur.
Projeler içindeki usingler GlobalUsing.cs classında tutulmuştur.
Proje içinde ayrı ayrı 3 proje mevcuttur bu projeler ;ExcelMessage, FileCreateWorkerService ve RabbitMQ_excelCreate projeleridir.
Kullanıcı bilgilendirmeleri SignalR ile gerçekleşmiştir.

Proje .Net core 8.0 versiyonuyla oluşturulmuştur.
