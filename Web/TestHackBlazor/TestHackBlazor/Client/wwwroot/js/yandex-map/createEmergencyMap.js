var myEmergMap;

function setupEmergYandexMap(arrayOfPoints, lat, long) {
    // Дождёмся загрузки API и готовности DOM.
    ymaps.ready(initYandexMap);

    function initYandexMap() {

        var array = [];
        var center = [55.76, 37.64]; // Москва
        var zoom = 10;
        if (arrayOfPoints != null) {
            array = JSON.parse(arrayOfPoints);
            center = array[0][0];
            zoom = 15;
        }
        // Создание экземпляра карты и его привязка к контейнеру с
        // заданным id ("map").
        myEmergMap = new ymaps.Map('myEmergMap', {
            // При инициализации карты обязательно нужно указать
            // её центр и коэффициент масштабирования.
            center: center, // Москва
            zoom: zoom
        }, {
            searchControlProvider: 'yandex#search'
        });

        // Создаем многоугольник без вершин.
        var myPolygon = new ymaps.Polygon(array, {}, {
            // Цвет заливки.
            fillColor: '#bdb3b7',
            // Цвет обводки.
            strokeColor: '#0000FF',

            fillOpacity: 0.4,

            // Ширина обводки.
            strokeWidth: 5
        });

        // Добавляем многоугольник на карту.
        myEmergMap.geoObjects.add(myPolygon);

        if (lat == null || long == null) {
            return;
        }

        myEmergMap.geoObjects.add(new ymaps.Placemark([lat, long], {
            balloonContent: 'Тревожный случай'
        }, {
            preset: 'islands#circleIcon',
            iconColor: 'red'
        }))

        
    }
}

