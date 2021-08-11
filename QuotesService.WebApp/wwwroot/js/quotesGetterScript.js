var quotesGetterApp = new Vue({
    el: '#quotesGetterApp',
    data: {
        markets: [],
        currentMarketName: '',
        currentTickerName: '',
        addedMarketName: '',
        addedTickerName: '',
        testVar: 0
    },
    computed: {
        currentMarketTickers: function () {
            const vue = this;

            if (vue.currentMarketName?.length > 0) {
                return vue.markets.filter(x => x.marketName === vue.currentMarketName)[0].tickersNames;
            }
            else {
                return [];
            }
        }
    },
    created: function () {
        const vue = this;

        vue.getMarkets();
    },
    methods: {
        getMarkets: function () {
            const vue = this;

            axios
                .get(getMarketsUrl)
                .then(response => {
                    vue.markets = response.data;
                })
                .catch(error => {
                    alert(error);
                });
        },
        addMarket: function () {
            const vue = this;

            if (vue.addedMarketName?.length > 0 === false) {
                alert('Имя добавляемого рынка не должно быть пустым!');
                return;
            }

            axios
                .get(addMarketUrl + '?marketName=' + vue.addedMarketName)
                .then(response => {
                    if (response.data?.isSuccess === true) {
                        alert('Рынок ' + vue.addedMarketName + ' успешно добавлен!');
                        vue.getMarkets();
                        vue.addedMarketName = '';
                    }
                    else {
                        alert('Ошибка: ' + response.data?.message ?? 'неизвестная ошибка сервера');
                    }
                })
                .catch(error => {
                    alert(error);
                });
        },
        deleteMarket: function () {
            const vue = this;

            if (vue.currentMarketName?.length > 0 === false) {
                return;
            }

            axios
                .get(deleteMarketUrl + '?marketName=' + vue.currentMarketName)
                .then(response => {
                    if (response.data?.isSuccess === true) {
                        alert('Рынок ' + vue.currentMarketName + ' успешно удален!');
                        vue.getMarkets();
                        vue.currentMarketName = '';
                    }
                    else {
                        alert('Ошибка: ' + response.data?.message ?? 'неизвестная ошибка сервера');
                    }
                })
                .catch(error => {
                    alert(error);
                });
        },
        addTicker: function () {
            const vue = this;

            if (vue.currentMarketName?.length > 0 === false) {
                alert('Рынок не выбран!');
                return;
            }

            if (vue.addedTickerName?.length > 0 === false) {
                alert('Имя добавляемого инструмента не должно быть пустым!');
                return;
            }

            let request = {};
            request.marketName = vue.currentMarketName;
            request.tickerName = vue.addedTickerName;

            axios
                .post(addTickerUrl, request)
                .then(response => {
                    if (response.data?.isSuccess === true) {
                        alert('Интсрумент ' + vue.addedTickerName + ' успешно добавлен!');
                        vue.getMarkets();
                        vue.addedTickerName = '';
                    }
                    else {
                        alert('Ошибка: ' + response.data?.message ?? 'неизвестная ошибка сервера');
                    }
                })
                .catch(error => {
                    alert(error);
                });
        },
        deleteTicker: function () {
            const vue = this;

            if (vue.currentMarketName?.length > 0 === false) {
                alert('Рынок не выбран!');
                return;
            }

            if (vue.currentTickerName?.length > 0 === false) {                
                return;
            }

            let request = {};
            request.marketName = vue.currentMarketName;
            request.tickerName = vue.currentTickerName;

            axios
                .post(deleteTickerUrl, request)
                .then(response => {
                    if (response.data?.isSuccess === true) {
                        alert('Интсрумент ' + vue.addedTickerName + ' успешно удален!');
                        vue.getMarkets();
                        vue.currentTickerName = '';
                    }
                    else {
                        alert('Ошибка: ' + response.data?.message ?? 'неизвестная ошибка сервера');
                    }
                })
                .catch(error => {
                    alert(error);
                });
        },
    }
});