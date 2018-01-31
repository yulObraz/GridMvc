    function EnumFilterWidget() {
        /***
        * This method must return type of registered widget type in 'SetFilterWidgetType' method
        */
        this.getAssociatedTypes = function () {
            return ["Enum"];
        };
        /***
        * This method invokes when filter widget was shown on the page
        */
        this.onShow = function () {
            /* Place your on show logic here */
        };

        this.showClearFilterButton = function () {
            return true;
        };
        /***
        * This method will invoke when user was clicked on filter button.
        * container - html element, which must contain widget layout;
        * lang - current language settings;
        * typeName - current column type (if widget assign to multipile types, see: getAssociatedTypes);
        * values - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
        * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
        * data - widget data passed from the server
        */
        this.onRender = function (container, lang, typeName, values, cb, data) {
            //store parameters:
            this.cb = cb;
            this.container = container;
            this.lang = lang;

            this.data = data;
            //this filterwidget demo supports only 1 filter value for column column
            if (values.length > 0) {
                this.value = values[0];
                if (this.value.filterValue) {
                    this.values = this.value.filterValue.split(",");
                } else {
                    this.values = [];
                }
            } else {
                this.value = { filterType: 1, filterValue: "" };
                this.values = [];
            }

            this.renderWidget(); //onRender filter widget
            this.registerEvents(); //handle events
        };
        this.renderWidget = function () {
            var options = "";
            var that = this;
            $.each(this.data, function(n, el) {
                options += "<option value='" + el.Value + "'" + (that.values.indexOf(el.Value)!=-1 ? "selected=\"selected\"" : "") + '>' + el.Text + '</option> '
            });

            var html =
                /*'<div class="form-group">\
                        <label>' + this.lang.filterTypeLabel + '</label>\
                        <select class="grid-filter-type form-control">\
                            <option value="1" ' + (this.value.filterType == "1" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.Equals + '</option>\
                            <option value="5" ' + (this.value.filterType == "5" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.GreaterThan + '</option>\
                            <option value="6" ' + (this.value.filterType == "6" ? "selected=\"selected\"" : "") + '>' + this.lang.filterSelectTypes.LessThan + '</option>\
                        </select>\
                    </div>' +*/
                    
                        '<div class="form-group">\
                        <p>Select some of values to filter:</p>\
                        <select class="form-control grid-enums" multiple="multiple">\
                        '+options+'\
                        </select>\
                    </div>';
            this.container.append(html);
        };
        this.registerEvents = function () {
            var list = this.container.find(".grid-enums");
            var filter = this.container.find(".grid-filter-type");
            var $context = this;
            list.change(function () {
                var values = [{ filterValue: $(this).val().join(","), filterType: 1 /* Equals */ }];
                $context.cb(values);
            });
        };
    }
    $.fn.gridmvcOld = $.fn.gridmvc;
    $.fn.gridmvc = function () {
        
        var grids = this.gridmvcOld();
        if (grids.length) {
            $.each(grids, function(n, el){el.addFilterWidget(new EnumFilterWidget());});
        } else {
            grids.addFilterWidget(new EnumFilterWidget());
        }
    };
