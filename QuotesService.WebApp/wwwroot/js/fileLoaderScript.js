var fileLoaderApp = new Vue({
    el: '#fileLoaderApp',
    data: {
        now: new Date(),
        fileName: '2.txt',
        loadedFileName: '',
        fileText: '',
        fileRows: [],
        removeRowsCount: 0,
        showRowsCount: 10,
        separator: '',
        dateTimeColumnNumber: '',
        openColumnNumber: '',
        hiColumnNumber: '',
        lowColumnNumber: '',
        closeColumnNumber: '',
        volumeColumnNumber: '',
        fields: ['Дата-время', 'Open', 'Hi', 'Low', 'Close', 'Volume'],
        dateTimeFormat: '',
        dateParsingYear: '',
        dateParsingMonth: '',
        dateParsingDay: '',
        dateParsingHour: '',
        dateParsingMin: '',
        dateParsingSec: '',
        dateParsingError: '',
        dateParsingSuccess: false
    },
    computed: {
        showRows: function () {
            return this.fileRows.slice(this.removeRowsCount, parseInt(this.removeRowsCount) + parseInt(this.showRowsCount));
        },
        testingRow: function () {
            const vue = this;

            return vue.parseRow(vue.showRows[0]);
        }
    },
    methods: {
        getFileText: function (fileName) {
            const vue = this;

            if (vue.fileName == vue.loadedFileName) {
                if (vue.loadedFileName.length > 0) {
                    alert('Файл ' + vue.loadedFileName + ' уже загружен!');
                }

                return;
            }

            axios
                .get('/load-from-file-api/get-file-text?fileName=' + fileName)
                .then(response => {

                    vue.fileText = response.data;
                    vue.fileRows = vue.fileText.split('\r\n');
                    vue.loadedFileName = vue.fileName;
                })
                .catch(error => {
                    vue.loadedFileName = '';
                    alert(error);
                });
        },
        getFieldValue: function (fieldName, text, debug) {
            const vue = this;
            let index = -1;

            switch (fieldName) {
                case vue.fields[0]: {
                    return vue.getDateTimeFieldValue(fieldName, vue.dateTimeColumnNumber, text, debug);
                }
                case vue.fields[1]: {
                    index = parseInt(vue.openColumnNumber);
                    break;
                }
                case vue.fields[2]: {
                    index = parseInt(vue.hiColumnNumber);
                    break;
                }
                case vue.fields[3]: {
                    index = parseInt(vue.lowColumnNumber);
                    break;
                }
                case vue.fields[4]: {
                    index = parseInt(vue.closeColumnNumber);
                    break;
                }
                case vue.fields[5]: {
                    index = parseInt(vue.volumeColumnNumber);
                    break;
                }
            }

            return vue.getFieldValueByIndex(fieldName, index, text, debug);
        },
        getDateTimeFieldValue: function (fieldName, indexes, text, debug) {
            const vue = this;

            let indexesArray = indexes.split(',');
            let result = '';

            for (i = 0; i < indexesArray.length; i++) {
                let index = parseInt(indexesArray[i]);
                result += vue.getFieldValueByIndex(fieldName, index, text, debug);
            }

            return result;
        },
        getFieldValueByIndex: function (fieldName, index, text, debug) {
            const vue = this;

            let result = '';
            let log = '';
            let rows = text.split(vue.separator);

            if (isNaN(index)) {
                log = 'index of ' + fieldName + ' is NaN';
            }
            else if (index < 0) {
                log = 'index of ' + fieldName + ' < 0';
            }
            else if (index >= rows.length) {
                log = 'index of ' + fieldName + ' > rows.length';
            }
            else {
                result = rows[index];
            }

            if (debug === true && log?.length > 0) {
                console.log(log);
            }

            return result;
        },
        parseRow: function (row) {
            const vue = this;

            row = row ?? '';

            let result = {};
            result.dateTime = vue.getFieldValue(vue.fields[0], row);
            result.open = vue.getFieldValue(vue.fields[1], row);
            result.hi = vue.getFieldValue(vue.fields[2], row);
            result.low = vue.getFieldValue(vue.fields[3], row);
            result.close = vue.getFieldValue(vue.fields[4], row);
            result.volume = vue.getFieldValue(vue.fields[5], row);

            return result;
        },
        tryParseDateTime: function (text, format) {
            const vue = this;

            if (!(text?.length > 0) || !(format?.length > 0)) {
                vue.setParseDateTimeResult(null);
                return;
            }

            let request = {};
            request.text = text;
            request.format = format;

            axios
                .post('/load-from-file-api/date-time-try-parse', request)
                .then(response => {
                    vue.setParseDateTimeResult(response);
                })
                .catch(error => {
                    vue.loadedFileName = '';
                    alert(error);
                });
        },
        setParseDateTimeResult: function (response) {
            const vue = this;

            if (response?.data?.success === true) {
                vue.dateParsingYear = response.data.year.toString();
                vue.dateParsingMonth = response.data.month > 9 ? response.data.month.toString() : '0' + response.data.month.toString();
                vue.dateParsingDay = response.data.day > 9 ? response.data.day.toString() : '0' + response.data.day.toString();
                vue.dateParsingHour = response.data.hour > 9 ? response.data.hour.toString() : '0' + response.data.hour.toString();
                vue.dateParsingMin = response.data.min > 9 ? response.data.min.toString() : '0' + response.data.min.toString();
                vue.dateParsingSec = response.data.sec > 9 ? response.data.sec.toString() : '0' + response.data.sec.toString();
                vue.dateParsingError = '';
                vue.dateParsingSuccess = true;
            }
            else {
                vue.dateParsingYear = '';
                vue.dateParsingMonth = '';
                vue.dateParsingDay = '';
                vue.dateParsingHour = '';
                vue.dateParsingMin = '';
                vue.dateParsingSec = '';
                vue.dateParsingError = response?.data?.error ?? '';
                vue.dateParsingSuccess = false;
            }
        }
    }
});