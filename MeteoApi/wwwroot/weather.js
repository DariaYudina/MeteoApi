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

            // Группируем данные по городам
            const cityDataMap = new Map();

            data.forEach(weatherInfo => {
                const city = weatherInfo.city;

                if (!cityDataMap.has(city)) {
                    cityDataMap.set(city, []);
                }

                cityDataMap.get(city).push(weatherInfo);
            });

            // Выводим данные для каждого города
            cityDataMap.forEach((cityData, city) => {
                const cityBlock = document.createElement('div');
                cityBlock.className = 'city-block';
                cityBlock.innerHTML = `<h2>${city}</h2>`;

                cityData.forEach(weatherInfo => {
                    const weatherItem = document.createElement('div');
                    weatherItem.className = 'weather-item';
                    weatherItem.innerHTML = `
                        <strong>Date:</strong> ${weatherInfo.date}<br>
                        <strong>Max Temperature:</strong> ${weatherInfo.maxTemperature}°C<br>
                        <strong>Min Temperature:</strong> ${weatherInfo.minTemperature}°C<br>
                        <strong>Wind:</strong> ${weatherInfo.wind}<br>
                        <strong>Summary:</strong> ${weatherInfo.summary}<br><br>
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
loadDataWithInterval();
