(function(){    
    var playlistRowTemplate = '<div class="row"><div class="col-sm-9 fileContainer"></div><div class="col-sm-offset-0 col-sm-3 text-right optionsContainer"></div></div>';
    var apiKey = location.search.substring(1);
    var serverSettings = {};
    var $playlist;

    function sendChange(path, data) {
        data.apiKey = apiKey;

        $.ajax({
            type: 'PUT',
            url: '/api/' + path,
            data: JSON.stringify(data)
        });
    }

    function getData(path) {
        path = path + (path.indexOf('?') > -1 ? '&' : '?') + 'apiKey=' + encodeURIComponent(apiKey);
        return $.ajax({
            type: 'get',
            url: '/api/' + path
        });
    }

    function playLink(e) {
        e.preventDefault();

        sendChange('play', { FileName: decodeURIComponent(this.pathname.substring(1)) });
    };

    function changeStartupFile(e) {
        e.preventDefault();

        var inputs = $('input[type="checkbox"].startupFile');
        var filename = this.value;
		
        inputs.not(this).attr('checked', false);

        sendChange('startupFile', { filename: this.checked ? filename : null });
    };

    function showNewKey(key) {
        apiKey = key;

        var url = (location.href.indexOf('?') > -1 ? location.href.substring(0, location.href.indexOf('?')) : '') + '?' + apiKey;
        

        var html = $('#apiKeyNotification').html()
        html = html.replace('{key}', key);
        html = html.replace('{url}', url);

        $('#apiKeyNotification').html(html);
        $('#apiKeyNotification').show();
        $('#apiKeyNotification a[data-dismiss="alert"]').on('click', function () {
            sendChange('/apikey/seen', {});
        });
    };

    function addPlaylistItem(file) {
        var $row = $(playlistRowTemplate);
        var $fileContainer = $row.find('.fileContainer');
        var $optionsContainer = $row.find('.optionsContainer');

        var $link = $('<a/>');
        var $input = $('<input type="checkbox" class="startupFile" />');
        var $label = $('<label class="float-right"></label>');

        $input.val(file.FileName);

        $link.attr('href', file.FileName);
        $link.text(file.DisplayName);

        $input.attr('checked', file.FileName === serverSettings.StartupFilename);

        $input.appendTo($label);
        $link.appendTo($fileContainer);
        $label.appendTo($optionsContainer);
        $row.appendTo($playlist);

        $link.on('click', playLink);
        $input.on('change', changeStartupFile);
    };

    $(document).ready(function () {
        $playlist = $('#playlist');

        var $volume = $('#volume');

        $.when(getData('settings'), getData('playlist'))
            .then(function (settings, playlist) {
                serverSettings = settings[0];
                playlist = playlist[0];

                if (serverSettings.Volume !== null) {
                    $volume.val(serverSettings.Volume);
                }

                if (!serverSettings.ApiKeySaved) {
                    showNewKey(serverSettings.ApiKey);
                }

                var files = playlist.Files;
                files.forEach(addPlaylistItem);
            });

        $volume.on('change', function (e) {
            sendChange('volume', { Volume: parseInt($(this).val()) });
        });;
    });
})();