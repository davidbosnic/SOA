import { Data } from "./data.js";

export class Dashboard
{
    constructor()
    {
        this.data = [];
        this.container = null;
    }

    addData(newData)
    {
        this.data.push(newData);
    }

    deleteAll()
    {
        this.data = [];
    }

    draw(parent){

        var dash = this;

        function createDiv(parentDiv, className, textContent)
        {
            var div = document.createElement("div");
            field1.className = className;
            field1.textContent = textContent;
            parentDiv.appendChild(div);
        }

        this.container = document.createElement("div");
        this.container.className = "parameterDiv";
        parent.appendChild(this.container);

        var form = document.createElement("div");
        form.className = "formDiv";
        this.container.appendChild(form);

        var elements = document.createElement("div");
        elements.className = "elementsDiv";
        this.container.appendChild(elements);

        var div1 = document.createElement("div");
        form.appendChild(div1);
        var input1 = document.createElement("input");
        input1.type = "button";
        input1.value = "Delete all";
        input1.className = "deleteAllInput";
        div1.appendChild(input1);
        input1.onclick = function(){
            fetch('https://localhost:1025/api/Gateway/RemoveAllData', {
                method: 'DELETE',
                })
                .then(res => res.text())
                .then(res => console.log(res))
        }

        var div2 = document.createElement("div");
        form.appendChild(div2);
        var input2 = document.createElement("input");
        input2.type = "button";
        input2.value = "Get all";
        input2.className = "getAllInput";
        div2.appendChild(input2);
        input2.onclick = function(){

            dash.deleteAll();
            fetch("https://localhost:1025/api/Gateway/GetAllSensorData", {
                method: "GET",
                
                headers: {
                    "Accept": "*/*",
                    "Connection":"keep-alive"
                  }
            }).then(p => p.json().then(data => {
                data.forEach(d => {
                    const newData = new Data(d["id"], d["value"], d["type"], d["recordTime"]);
                    dash.addData(newData);
                });
                dash.drawElements(elements);
            }));
            
        }

        var div3 = document.createElement("div");
        form.appendChild(div3);
        var input3 = document.createElement("input");
        input3.type = "button";        
        input3.value = "Get special type";
        input3.className = "getSpecialType";
        div3.appendChild(input3);
        var sel = document.createElement("select");
        div3.appendChild(sel);
        const array = ["temperature", "cloud", "pressure", "humidity", "rainfall", "windspeed"];
        array.forEach(element => {
            var op = document.createElement("option");
            op.value = element;
            op.text = element;
            op.selected = "selected";
            sel.add(op);
        });
        input3.onclick = function(){
            dash.deleteAll();
            console.log(sel.options[sel.selectedIndex].value);
            fetch("https://localhost:1025/api/Gateway/GetAllTypedSensorData?typeOfSensor="+sel.options[sel.selectedIndex].value, {
                method: "GET",
                                
                headers: {
                    "Accept": "*/*",
                    "Connection":"keep-alive"
                  }
            }).then(p => p.json().then(data => {
                data.forEach(d => {
                    const newData = new Data(d["id"], d["value"], d["type"], d["recordTime"]);
                    dash.addData(newData);
                });
                dash.drawElements(elements);
            }))
            .catch(error => console.log(error) );

        }

        this.drawElements(elements);

    }

    drawElements(elements){
        elements.innerHTML = "";
        this.data.forEach(element => {
            element.draw(elements);
        });
    }
}