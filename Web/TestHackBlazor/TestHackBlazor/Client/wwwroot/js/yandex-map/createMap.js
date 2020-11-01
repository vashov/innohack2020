var myMap;

function setupYandexMap(dotNetReference, arrayOfPoints) {
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
        myMap = new ymaps.Map('mymap', {
            // При инициализации карты обязательно нужно указать
            // её центр и коэффициент масштабирования.
            center: center, // Москва
            zoom: zoom
        }, {
            searchControlProvider: 'yandex#search'
        });

        // Создаем многоугольник без вершин.
        

        var myPolygon = new ymaps.Polygon(array, { }, {
            // Курсор в режиме добавления новых вершин.
            editorDrawingCursor: "crosshair",
            // Максимально допустимое количество вершин.
            editorMaxPoints: 8,
            // Цвет заливки.
            fillColor: '#bdb3b7',
            // Цвет обводки.
            strokeColor: '#0000FF',

            fillOpacity: 0.4,

            // Ширина обводки.
            strokeWidth: 5
        });

        //myPolygon.events.add([
        //    'mapchange', 'geometrychange', 'pixelgeometrychange', 'optionschange', 'propertieschange',
        //    'balloonopen', 'balloonclose', 'hintopen', 'hintclose', 'dragstart', 'dragend'
        //], function (e) {
        //    log.innerHTML = '@' + e.get('type') + '<br/>' + log.innerHTML;
        //});

        //myPolygon.events.add([
        //    'mapchange', 'geometrychange', 'pixelgeometrychange', 'optionschange', 'propertieschange',
        //    'balloonopen', 'balloonclose', 'hintopen', 'hintclose', 'dragstart', 'dragend'
        //], function (e) {
        //        console.log(e.get('type'))
        //});

        myPolygon.events.add('geometrychange',
            function (e) {
                //console.log(myPolygon.geometry.getCoordinates());
                var coodinates = myPolygon.geometry.getCoordinates();

                dotNetReference.invokeMethodAsync('OnCoordinatesChanged', coodinates);
            });

        //myPolygon.events.add('optionschange',
        //    function (e) {
        //        console.log(e.get('type'));
        //    });

        // Добавляем многоугольник на карту.
        myMap.geoObjects.add(myPolygon);

        // В режиме добавления новых вершин меняем цвет обводки многоугольника.
        var stateMonitor = new ymaps.Monitor(myPolygon.editor.state);
        stateMonitor.add("drawing", function (newValue) {
            myPolygon.options.set("strokeColor", newValue ? '#FF0000' : '#4538ff');
        });

        // Включаем режим редактирования с возможностью добавления новых вершин.
        myPolygon.editor.startDrawing();

        //document.getElementById('destroyButton').onclick = function () {
        //    // Для уничтожения используется метод destroy.
        //    myMap.destroy();
        //};

    }
}
