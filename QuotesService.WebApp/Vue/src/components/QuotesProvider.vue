<template>
    <div>
        <div v-if="currentQuotesProvider?.assigned === true">
            <h4>Поставщик котировок установлен</h4>
            <div class="border">
                <h4>Поставщик котировок</h4>
                <input type="text" class="form-control" readonly="true" v-model="currentQuotesProvider.assignedName" />
                <div class="border">
                    <h4>Параметры</h4>
                    <div v-for="(item, index) in currentQuotesProvider?.parameters" :key="item.value">
                        <label v-bind:for="'quotesProviderProperty' + index" class="form-label">
                            <strong>{{ item.key }}</strong>
                        </label>
                        <input type="text" class="form-control" v-bind:id="'quotesProviderProperty' + index" readonly="true" v-model="item.value" />
                    </div>
                </div>
            </div>
        </div>

        <div v-else>
            <h4>Поставщик котировок не назначен</h4>
            <h4>Поставщики котировок</h4>
            <form class="row g-3">
                <div class="col-md-8">
                    <select class="form-select form-select-lg" v-model="currentQuotesProvider.assignedName">
                        <option disabled value="">Выбор поставщика котировок</option>
                        <option v-for="item in currentQuotesProvider?.allQuotesProviders?.map((x) => x.quotesProviderName)" :key="item">
                            {{ item }}
                        </option>
                    </select>
                </div>

                <div class="col-md-2">
                    <button type="button" class="btn btn-primary" v-on:click="getQuotesProviderParameters()">
                        Получить параметры
                    </button>
                </div>
            </form>

            <div v-for="(item, index) in currentQuotesProvider?.parameters" :key="item.value">
                <label v-bind:for="'quotesProviderProperty' + index" class="form-label">
                    <strong>{{ item.key }}</strong>
                </label>

                <input type="text" class="form-control" v-bind:id="'quotesProviderProperty' + index" v-model="item.value" />
            </div>

            <form class="row g-3">
                <div class="col-md-1" v-if="currentQuotesProvider?.parameters?.length > 0">
                    <button type="button" class="btn btn-primary" v-on:click="checkGetQuotes()">
                        Проверить
                    </button>
                </div>

                <div class="col-md-1" v-if="currentQuotesProvider?.parameters?.length > 0">
                    <button type="button" class="btn btn-primary" v-on:click="setQuotesProviderParameters()">
                        Сохранить
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "QuotesProvider",
    props: {
        currentMarketNameIP: String,
        currentTickerNameIP: String,
    },
    emits:["ticker-renamed"],
    data: function() {
        return {
            checkGetQuotesUrl: "/quotes-getter-api/check-get-quotes",
            getQuotesUrl: "/quotes-getter-api/get-quotes",
            getQuotesProviderUrl: "/quotes-getter-api/get-quotes-provider",
            getQuotesProviderParametersUrl: "/quotes-getter-api/get-quotes-provider-parameters",
            setQuotesProviderParametersUrl: "/quotes-getter-api/set-quotes-provider-parameters",
            currentQuotesProvider: {},
            currentMarketName: this.currentMarketNameIP,
            currentTickerName: this.currentTickerNameIP,
        };
    },
    created: function() {
        this.getQuotesProvider();
    },
    watch: {
        currentMarketNameIP: function() {
            this.currentMarketName = this.currentMarketNameIP;
            this.currentTickerName = this.currentTickerNameIP;
            this.getQuotesProviderTasks();
        },
        currentTickerNameIP: function() {
            this.currentTickerName = this.currentTickerNameIP;
            this.getQuotesProvider();
        },
    },
    methods: {
        getQuotesProvider: function() {
            if (this.currentMarketName?.length > 0 === false || this.currentTickerName?.length > 0 === false) {
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;

            this.currentQuotesProvider = {};

            axios
                .post(this.getQuotesProviderUrl, request)
                .then((response) => {
                    this.currentQuotesProvider.assigned = response.data?.quotesProviderAssigned;
                    this.currentQuotesProvider.allQuotesProviders = response.data?.allQuotesProviders;

                    if (response.data?.quotesProviderAssigned === true) {
                        this.currentQuotesProvider.assignedName = response.data?.currentQuotesProvider?.quotesProviderName;

                        //get properties
                        this.getQuotesProviderParameters();
                    } else {
                        this.currentQuotesProvider.assignedName = "";
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        getQuotesProviderParameters: function() {
            if (
                this.currentMarketName?.length > 0 === false ||
                this.currentTickerName?.length > 0 === false ||
                this.currentQuotesProvider?.assignedName?.length > 0 == false
            ) {
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;
            request.quotesProviderType = this.getQuotesProviderType(this.currentQuotesProvider);

            axios
                .post(this.getQuotesProviderParametersUrl, request)
                .then((response) => {
                    this.currentQuotesProvider.parameters = response.data;
                })
                .catch((error) => {
                    alert(error);
                });
        },
        checkGetQuotes: function() {
            let request = {};
            request.quotesProviderType = this.getQuotesProviderType(this.currentQuotesProvider);
            request.parameters = this.currentQuotesProvider?.parameters;

            axios
                .post(this.checkGetQuotesUrl, request)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Тестовые котировки успешно получены!");
                    } else {
                        alert(response.data?.message ?? "Неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        setQuotesProviderParameters: function() {
            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;
            request.quotesProviderType = this.getQuotesProviderType(this.currentQuotesProvider);
            request.parameters = this.currentQuotesProvider?.parameters;

            axios
                .post(this.setQuotesProviderParametersUrl, request)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Параметры успешно сохранены!");

                        //Обновление информации                        
                        this.$emit("ticker-renamed", response.data.message);
                    } else {
                        alert(response.data?.message ?? "Неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },

        getQuotesProviderType: function(quotesProvider) {
            return quotesProvider?.allQuotesProviders
                ?.filter((x) => x.quotesProviderName === quotesProvider?.assignedName)
                ?.map((x) => x.quotesProviderType)
                ?.find((x) => x !== undefined);
        },
    },
};
</script>

<style scoped>
button {
    margin: 0.8vmin 0px 0.8vmin 0px;
}
</style>
