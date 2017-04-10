(function(){    
    var playlistRowTemplate = '<div class="row"><div class="col-sm-9 fileContainer"></div><div class="col-sm-offset-0 col-sm-3 text-right optionsContainer"></div></div>';
    var apiKey = location.search.substring(1);

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

    $(document).ready(function () {
        var $playlist = $('#playlist');
        var $volume = $('#volume');

        $.when(getData('settings'), getData('playlist'))
            .then(function (settings, playlist) {
                settings = settings[0];
                playlist = playlist[0];

                if (settings.Volume !== null) {
                    $volume.val(settings.Volume);
                }

                var files = playlist.Files;

                for (var i = 0; i < files.length; i++) {
					var file = files[i];
					
                    var $row = $(playlistRowTemplate);
                    var $fileContainer = $row.find('.fileContainer');
                    var $optionsContainer = $row.find('.optionsContainer');
                    
                    var $link = $('<a/>');
                    var $input = $('<input type="checkbox" class="startupFile" />');
                    var $label = $('<label class="float-right"></label>');
                    
                    $input.val(file.FileName);

                    $link.attr('href', file.FileName);
                    $link.text(file.DisplayName);

                    $input.attr('checked', file.FileName === settings.StartupFilename);

                    $input.appendTo($label);
                    $link.appendTo($fileContainer);
                    $label.appendTo($optionsContainer);
                    $row.appendTo($playlist);

                    $link.on('click', playLink);
                    $input.on('change', changeStartupFile);
                }
            });

        $volume.on('change', function (e) {
            sendChange('volume', { Volume: parseInt($(this).val()) });
        });;
    });
})();