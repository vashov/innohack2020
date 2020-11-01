var myConstructsMap;

function setupConstructsYandexMap(arrayOfArrays) {
    // Дождёмся загрузки API и готовности DOM.
    ymaps.ready(initYandexMap);

    function initYandexMap() {
        console.log(arrayOfArrays);

        myConstructsMap = new ymaps.Map('myConstructsMap', {
            // При инициализации карты обязательно нужно указать
            // её центр и коэффициент масштабирования.
            center: [55.76, 37.64], // Москва
            zoom: 10
        }, {
            searchControlProvider: 'yandex#search'
        });

        var strArr = String(arrayOfArrays);

        strArr.split(";").forEach( (val, idx) => {

            var array = [];
            if (val != null) {
                array = JSON.parse(val);
            }

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
            myConstructsMap.geoObjects.add(myPolygon);
        });

    }
}

