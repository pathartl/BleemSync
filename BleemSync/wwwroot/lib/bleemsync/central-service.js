class BleemSyncCentral {
    static GetBaseUrl() {
        let baseUrl = "https://central.bleemsync.app/api";

        return baseUrl;
    }

    static GetGameInfoByFingerprint(fingerprint, system) {
        // Does the routing for BleemSync Central metadata retrieval
        let request = new XMLHttpRequest();
        let endpoint = "";

        switch (system) {
            case "PlayStation":
                endpoint = `PlayStation/GetBySerial/${fingerprint}`;
                break;
        }

        request.open("GET", `${this.GetBaseUrl()}/${endpoint}`, false);
        request.send(null);

        if (request.status === 200) {
            console.log(request.responseText);
            var game = JSON.parse(request.responseText);

            if (game.Name == null && game.Title != null) game.Name = game.Title;
            if (game.SortName == null && game.CommonTitle != null) game.SortName = game.CommonTitle;
            if (game.ReleaseDate == null && game.DateReleased != null) game.ReleaseDate = game.DateReleased.split('T')[0];

            return game;
        } else {
            throw new DOMException(`Could not scrape data for a game with the fingerprint of ${fingerprint} for the system ${system}.`);
        }
    }

    static GetCoverUrlByFingerprint(fingerprint, system) {
        let endpoint = "";

        switch (system) {
            case "PlayStation":
                endpoint = `PlayStation/GetCoverBySerial/${fingerprint}`;
                break;
        }

        return `${this.GetBaseUrl()}/${endpoint}`
    }

    static GetCoverFileByFingerprint(fingerprint, system) {
        // Does the routing for BleemSync Central cover retrieval
        let request = new XMLHttpRequest();
        let endpoint = this.GetCoverUrlByFingerprint(fingerprint, system);

        request.open("GET", endpoint, false);
        request.responseType = "blob";
        request.send(null);

        let blob = request.response;

        blob.lastModifiedDate = new Date();
        blob.name = "cover.jpg";

        return blob;
    }
}