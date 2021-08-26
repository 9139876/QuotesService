<template>
    <div>
        <div v-if="currentQuotesProvider.assigned">
            <h4>Поставщик котировок: {{ currentQuotesProvider.assignedName }}</h4>

            <div v-if="currentQuotesProviderTasks?.quotesProviderTasks?.length > 0">
                <div v-for="(item, index) in currentQuotesProviderTasks?.quotesProviderTasks" :key="index">
                    <div class="border">
                        <input class="form-check-input" type="checkbox" id="'checkBox-'+index" v-model="item.isActive" />
                        <label class="form-check-label" for="'checkBox-'+index">
                            <strong>Активно</strong>
                        </label>

                        <form class="row g-3">
                            <div class="col-md-4">
                                <label :for="'tf' + index" class="form-label">
                                    <strong>Таймфрейм</strong>
                                </label>
                                <input type="text" class="form-control" :id="'tf' + index" readonly="true" v-model="item.timeFrameName" />
                            </div>

                            <div class="col-md-3">
                                <label :for="'updatePeriod' + index" class="form-label">
                                    <strong>Период обновления (сек)</strong>
                                </label>
                                <input type="text" class="form-control" :id="'updatePeriod' + index" v-model="item.updatePeriodInSecond" />
                                <div class="error-text" v-if="item.correctInput === false">
                                    Значение дожно быть целым числом от 1 до {{ maxUpdatePeriodValue }}.
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label :for="'lastUpdateDate' + index" class="form-label">
                                    <strong>Последнее обновление</strong>
                                </label>
                                <input
                                    type="text"
                                    class="form-control"
                                    :id="'lastUpdateDate' + index"
                                    readonly="true"
                                    v-model="item.lastUpdateDate"
                                />
                            </div>
                        </form>
                    </div>
                </div>

                <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                    <button type="button" class="btn btn-primary" data-bs-dismiss="modal" v-on:click="setQuotesProviderTasks()">
                        Сохранить изменения
                    </button>
                </div>
            </div>
            <div v-else>
                <div><strong>Не найдено ни одной задачи</strong></div>
            </div>
        </div>

        <div v-else>
            <h4>Поставщик котировок не назначен</h4>
        </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
    name: "QuotesProviderTasks",
    props: {
        currentMarketNameIP: String,
        currentTickerNameIP: String,
    },
    data: function() {
        return {
            currentQuotesProvider: {},
            currentQuotesProviderTasks: {},
            getQuotesProviderUrl: "/quotes-getter-api/get-quotes-provider",
            getQuotesProviderTasksUrl: "/quotes-provider-tasks/get-quotes-provider-tasks",
            setQuotesProviderTasksUrl: "/quotes-provider-tasks/set-quotes-provider-tasks",
            currentMarketName: this.currentMarketNameIP,
            currentTickerName: this.currentTickerNameIP,
            maxUpdatePeriodValue: 86400,
        };
    },
    created: function() {
        this.getQuotesProvider();
        this.getQuotesProviderTasks();
    },
    watch: {
        currentMarketNameIP: function() {
            this.currentMarketName = this.currentMarketNameIP;
            this.currentTickerName = this.currentTickerNameIP;
            this.getQuotesProviderTasks();
        },
        currentTickerNameIP: function() {
            this.currentTickerName = this.currentTickerNameIP;
            this.getQuotesProviderTasks();
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

                    if (response.data?.quotesProviderAssigned === true) {
                        this.currentQuotesProvider.assignedName = response.data?.currentQuotesProvider?.quotesProviderName;
                    } else {
                        this.currentQuotesProvider.assignedName = "";
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        getQuotesProviderTasks: function() {
            if (this.currentMarketName?.length > 0 === false || this.currentTickerName?.length > 0 === false) {
                return;
            }

            let request = {};
            request.marketName = this.currentMarketName;
            request.tickerName = this.currentTickerName;

            this.currentQuotesProviderTasks = {};

            axios
                .post(this.getQuotesProviderTasksUrl, request)
                .then((response) => {
                    if (response.data?.status?.isSuccess === true) {
                        this.currentQuotesProviderTasks = response.data;

                        for (let task of this.currentQuotesProviderTasks?.quotesProviderTasks) {
                            task.correctInput = true;
                        }
                    } else {
                        alert("Ошибка: " + response.data?.status?.message ?? "неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        setQuotesProviderTasks: function() {
            if (this.validateInputs() !== true) {
                return;
            }

            axios
                .post(this.setQuotesProviderTasksUrl, this.currentQuotesProviderTasks)
                .then((response) => {
                    if (response.data?.isSuccess === true) {
                        alert("Изменения успешно сохранены");
                    } else {
                        alert("Ошибка: " + response.data?.message ?? "неизвестная ошибка сервера");
                    }
                })
                .catch((error) => {
                    alert(error);
                });
        },
        validateInputs: function() {
            let result = true;

            for (let task of this.currentQuotesProviderTasks?.quotesProviderTasks) {
                if (
                    isNaN(task.updatePeriodInSecond) === false &&
                    task.updatePeriodInSecond > 0 &&
                    task.updatePeriodInSecond <= this.maxUpdatePeriodValue
                ) {
                    task.correctInput = true;
                } else {
                    task.correctInput = false;
                    result = false;
                }
            }

            return result;
        },
    },
};
</script>

<style scoped>
button {
    margin: 0.8vmin 0px 0.8vmin 0px;
}
.form-check-input {
    margin: 0.5vmin 0.5vmin 0.5vmin 0px;
}
.error-text {
    color: red;
}
</style>
