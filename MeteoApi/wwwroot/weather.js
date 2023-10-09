// Функция для форматирования даты
function formatDate(dateString) {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU', options);
}

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
            // Очищаем контейнер с данными
            const cityContainer = document.getElementById('cityContainer');
            cityContainer.innerHTML = '';

            // Выводим данные для каждого города
            data.forEach(cityData => {
                const cityBlock = document.createElement('div');
                cityBlock.className = 'city-block';
                cityBlock.innerHTML = `<h2>${cityData.City}</h2>`;

                cityData.WeatherEntries.forEach(weatherInfo => {
                    const weatherItem = document.createElement('div');
                    weatherItem.className = 'weather-item';
                    weatherItem.innerHTML = `
                        <strong>Date:</strong> ${formatDate(weatherInfo.Date)}<br>
                        <strong>Max Temperature:</strong> ${weatherInfo.MaxTemperature}°C<br>
                        <strong>Min Temperature:</strong> ${weatherInfo.MinTemperature}°C<br>
                        <strong>Wind:</strong> ${weatherInfo.Wind}<br>
                        <strong>Summary:</strong> ${weatherInfo.Summary}<br><br>
                    `;

                    cityBlock.appendChild(weatherItem);
                });

                cityContainer.appendChild(cityBlock);
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

// Вызываем функцию для загрузки данных
loadDataWithInterval();
