$(document).ready(function () {
    // Sayfadaki 'datatable-ajax' sınıfına sahip TÜM tabloları bul ve her biri için bu fonksiyonu çalıştır.
    $('.datatable-ajax').each(function () {

        var table = $(this); // Mevcut tabloyu bir değişkene al
        var ajaxUrl = table.data('ajax-url'); // Tag Helper ile oluşturduğumuz data-ajax-url attribute'ünü oku

        // Eğer URL belirtilmemişse, bu tablo için işlem yapma.
        if (!ajaxUrl) {
            console.error('DataTables için data-ajax-url attribute\'ü bulunamadı.');
            return;
        }

        var columnsConfig = [];
        // Tablonun başlığındaki (thead) her bir th etiketini bul
        table.find('thead th').each(function () {
            var columnName = $(this).data('name');
            if (columnName) {
                // Eğer th'nin bir 'data-name' attribute'ü varsa,
                // columns yapılandırmasına ekle.
                columnsConfig.push({ data: columnName });
            } else {
                columnsConfig.push({ data: null, orderable: false, searchable: false });
            }
        });

        // DataTables'ı merkezi ayarlarla başlat.
        table.DataTable({
            // === TEMEL AYARLAR ===
            processing: true, // Veri yüklenirken "işleniyor" göstergesi
            serverSide: true, // Sunucu taraflı işlemeyi etkinleştir
            ajax: {
                url: ajaxUrl, // Veri çekilecek URL
                type: 'POST', // Güvenlik için POST metodu önerilir
                dataType: 'json'
            },

            // === GÖRSEL VE KULLANIM AYARLARI (Tüm tablolarda aynı olacak) ===
            responsive: true, // Mobil uyumluluk
            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Tümü"]], // Satır sayısı menüsü
            columns: columnsConfig, 
            //language: { // Türkçe dil ayarları
            //    url: "//cdn.datatables.net/plug-ins/1.10.25/i18n/Turkish.json"
            //},
            order: [[0, 'asc']]
        });
    });
});