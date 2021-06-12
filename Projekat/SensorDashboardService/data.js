export class Data{
    
    constructor(id, value, type, time){
        this.id = id;
        this.value = value;
        this.type = type;
        this.time = time;
        this.container = null;
    }

    draw(parent){

        function createDiv(parentDiv, className, textContent)
        {
            var div = document.createElement("div");
            div.className = className;
            div.textContent = textContent;
            parentDiv.appendChild(div);
        }

        this.container = document.createElement("div");
        this.container.className = "parameterDiv" + this.id;
        parent.appendChild(this.container);

        createDiv(this.container, "parameterField", "Value: " + this.value);
        createDiv(this.container, "parameterField", "Parameter type: " + this.type);
        createDiv(this.container, "parameterField", "Record time: "+ this.time);

    }
}