(function(){    
    var playlistRowTemplate = '<div class="row"><div class="col-sm-9 fileContainer"></div><div class="col-sm-offset-0 col-sm-3 text-right optionsContainer"></div></div>';
    var apiKey = location.search.substring(1);
    var serverSettings = {};
    var $playlist;

    axios.defaults.baseURL = '/api/';

    axios.interceptors.request.use(function (config) {
        config.url = config.url + (config.url.indexOf('?') > -1 ? '&' : '?') + 'apiKey=' + encodeURIComponent(apiKey);

        if (config.method == 'post' || config.method == 'put') {
            config.data.apiKey = apiKey;
        }

        return config;
    });

    function sendChange(path, data) {
        return axios.put(path, data).then(function (response) {
            return response.data;
        }).catch(function (error) {
            return Promise.reject(error);
        });
    }

    function getData(path) {
        return axios.get(path).then(function (response) {
            return response.data;
        }).catch(function (error) {
            return Promise.reject(error);
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

        var url = (location.href.indexOf('?') > -1 ? location.href.substring(0, location.href.indexOf('?')) : location.href) + '?' + apiKey;
        

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

        getData('settings').then(function (settings) {
            if (!settings.ApiKeySaved) {
                showNewKey(settings.ApiKey);
                apiKey = settings.ApiKey;
            }
            if (settings.Volume !== null) {
                $volume.val(settings.Volume);
            }

            getData('playlist').then(function (playlist) {
                var files = playlist.Files;
                files.forEach(addPlaylistItem);

            });
        }).catch(function (error) {
            return;
        });;

        $volume.on('change', function (e) {
            sendChange('volume', { Volume: parseInt($(this).val()) });
        });;
    });
})();