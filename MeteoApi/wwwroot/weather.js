// Функция для отправки GET запроса и обработки данных
function fetchData() {
    fetch('/api/weather', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        }
    })
        .then(response => response.json())
        .then(data => {
            // Обработка данных и отображение на странице
            const weatherDataElement = document.getElementById('weatherData');
            const cityNameElement = document.getElementById('cityName'); // Элемент для названия города

            // Устанавливаем название города только один раз
            if (cityNameElement.innerHTML === '') {
                cityNameElement.innerHTML = `<h4>${data[0].city} прогноз погоды:</h2>`;
            }

            weatherDataElement.innerHTML = ''; // Очищаем блок перед добавлением новых данных

            data.forEach(item => {
                const weatherItem = document.createElement('div');
                weatherItem.innerHTML = `<strong>Date:</strong> ${item.date}, <strong>Temperature:</strong> ${item.temperatureC}°C, <strong>Summary:</strong> ${item.summary}`;
                weatherDataElement.appendChild(weatherItem);
            });
        })
        .catch(error => {
            console.error('Ошибка при получении данных:', error);
        });
}

// Функция для загрузки данных и установки интервала
function loadDataWithInterval() {
    fetchData();
    setInterval(fetchData, 3600000);
}
loadDataWithInterval();
