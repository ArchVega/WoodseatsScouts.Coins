const unknownScoutImage = "images/unknown-scout-image.jpg"

function CoinModel() {
    this.code = ''
    this.baseNumber = ko.observable(0)
    this.pointValue = ko.observable(0)

    this.displayText = ko.computed(function () {
        return this.pointValue() + " pts";
    }, this);
}

function ViewModel() {
    const self = this
    self.scoutCode = ko.observable('')
    self.scoutName = ko.observable('')
    self.scoutTroopNumber = ko.observable('')
    self.scoutSection = ko.observable('')
    self.lastScannedCoinCode = ko.observable('')
    self.scoutPhotoPath = ko.observable(unknownScoutImage)
    self.showUserSection = ko.observable(false)
    self.scannedCoins = ko.observableArray([])
    self.scanCoinFieldEnabled = ko.observable('false')
    self.errorMessage = ko.observable('')
    self.toaster = $('.toast')
    self.confirmationMessage = ko.observable('')

    self.updateScoutDetailsOnPaste = (model, event) => {
        event.preventDefault();
        const scoutCode = (event.originalEvent.clipboardData || window.clipboardData).getData('text');
        model.scoutCode(scoutCode)

        const url = `/Home/GetScoutInfoFromCode?code=${scoutCode}`
        $.get({
            url: url,
            success: (result) => {
                setTimeout(() => {
                    model.scoutName(result.scoutName)
                    model.scoutPhotoPath(result.scoutPhotoPath)
                    model.scoutTroopNumber(result.scoutTroopNumber)
                    model.scoutSection(result.scoutSection)
                    model.showUserSection(true)
                }, 250)
            },
            error: () => {
                self.errorMessage(`An error occurred while processing code '${scoutCode}'. It could be a general error or the code is valid but the scout is not in the system.`)
                self.toaster.toast('show')
                model.scoutPhotoPath(unknownScoutImage)
                model.scoutTroopNumber('')
                model.scoutSection('')
                model.showUserSection(false)
            }
        })

        return true
    }

    self.addScannedCoinOnPaste = (model, event) => {
        event.preventDefault();
        const lastScannedCoinCode = (event.originalEvent.clipboardData || window.clipboardData).getData('text');
        model.lastScannedCoinCode(lastScannedCoinCode)
        const url = `/Home/GetPointValueFromCode?code=${lastScannedCoinCode}`
        $.get({
            url: url,
            success: (result) => {
                model.scanCoinFieldEnabled(false)
                setTimeout(() => {
                    const newCoin = new CoinModel()
                    newCoin.code = lastScannedCoinCode
                    newCoin.baseNumber(result.baseNumber)
                    newCoin.pointValue(result.pointValue)
                    
                    model.scannedCoins.push(newCoin)

                    model.scanCoinFieldEnabled(true)
                    model.lastScannedCoinCode("")
                }, 250)
            },
            error: () => {
                model.errorMessage(`An error occurred while processing code '${lastScannedCoinCode}'. It could be a general error or the code is not valid.`)
                self.toaster.toast('show')
            }
        })

        return true
    }

    self.confirmRemovePoint = function (coin) {
        self.scannedCoins.remove(coin)
    }

    self.totalPoints = ko.computed(() => {
        let total = 0;

        this.scannedCoins().forEach(function (x) {
            total += x.pointValue()
        })

        return total;
    }, this);

    self.scoutTroopNumberAndSection = ko.computed(() => {        
        return `Troop #: ${this.scoutTroopNumber()}, Section: ${this.scoutSection()}`;
    }, this);

    self.confirmSubmit = () => {
        const scoutName = this.scoutName()
        const totalPoints = this.totalPoints()
        const message = `Submit coins with a total of ${totalPoints} points for scout ${scoutName}?`
        if (confirm(message) === true) {
            let coinCodes = [];
            this.scannedCoins().forEach(function (x) {
                coinCodes.push(x.code)
            })

            const payload = {
                scoutCode: this.scoutCode(),
                coinCodes: coinCodes
            }

            const url = `/Home/AddPointsToScout`
            $.post(url, payload, function() {
                window.location.reload()                
            });
        } 
    }
}

const viewModel = new ViewModel();
ko.applyBindings(viewModel)