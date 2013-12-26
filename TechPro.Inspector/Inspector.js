(function () {
    var self = this;

    function build(obj, el) {
        el.innerHTML = '';
        el.className = "json-explorer"

        el.addEventListener('click', function (e) {
            if (e.target.classList.contains('parent')) {
                e.target.classList.toggle('expanded')
            }
        })

        function buildList(parentObj, parentEl) {
            var rEl, rParent;

            rParent = document.createElement('ul');

            for (var key in parentObj) {
                rEl = document.createElement('li');

                if (typeof parentObj[key] == "object") {
                    rEl.className = parentObj[key] instanceof Array ? 'parent array' : 'parent';
                    rEl.innerHTML = '<div class="key">' + key + ' [&plusmn;]</div>';
                    buildList(parentObj[key], rEl)
                }
                else {
                    rEl.innerHTML = '<div class="key">' + key + '</div><div class="value">' + parentObj[key] + '</div>';
                }

                rParent.appendChild(rEl);
            }

            parentEl.appendChild(rParent);
        }

        buildList(obj, el);
    }

    function onload() {
        console.log("Inspector booting up");

        // Wrapper element
        var wrapper = document.getElementById('tpinspector'),
             header = wrapper.children[0],
               main = wrapper.children[1],
          collapser = header.children[0],
           jsonData = JSON.parse(wrapper.getAttribute("data-json"));

        // Fold-In / Fold-Out Button
        collapser.onclick = function (evt) {    
            if (collapser.textContent === '◈') {
                collapser.textContent = '◇'
                collapser.setAttribute('title', 'Fold Out');
                main.style.display = 'none';
            } else {
                collapser.textContent = '◈'
                collapser.setAttribute('title', 'Fold In');
                main.style.display = 'block';
            }
        };

        // Remove the original data attribute
        wrapper.removeAttribute('data-json');

        // Build the DOM
        build(jsonData, main);
        collapser.onclick();
    }

    window.addEventListener('load', onload, false);
})();