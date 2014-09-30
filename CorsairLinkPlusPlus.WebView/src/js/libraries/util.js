"use strict";
var util = {};

util.makeSingletonGetter = function(classObject) {
	classObject.getInstance = function() {
		if(!classObject.instance)
			return classObject.instance = new classObject();
		return classObject.instance;
	};
};

util.checkInterface = function(implementation, wantedInterface) {
	for(var idx in wantedInterface.methods) {
		if(!implementation.prototype[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx);
		if(implementation[idx] != wantedInterface.methods[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx + " correctly");
	}
};

util.simulateClick = function(element) {
	var event = document.createEvent("MouseEvents"); 
	event.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null); 
	element.dispatchEvent(event);
};

util.simulateFormSubmit = function(form) {
	var submitButton = util.makeElement("input", {type: "submit"});
	this.simulateClick(form.appendChild(submitButton));
	form.removeChild(submitButton);
};

util.enableEnterToSubmit = function(form) {
	form.appendChild(
		util.makeElement("input", {
			type: "submit",
			style: "height: 0px; width: 0px; border: none; padding: 0px;",
			hidefocus: true
		})
	);
};

util.preventFormSubmission = function(form) {
	form.addEventListener("submit", function(event) {
		event.preventDefault();
		event.stopPropagation();
		return false;
	}, true);
};

util.checkAbstract = function(implementation, abstractParent) {
	for(var idx in abstractParent.abstracts) {
		if(!implementation.prototype[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx);
		if(implementation.prototype[idx].length != abstractParent.abstracts[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx + " correctly");
	}
};

util.removeFromArray = function(arr, obj) {
	arr.splice(arr.indexOf(obj), 1);
};

util.getOccurencesInString = function(str, what) {
	return str.split(what).length - 1;
};

util.arrayToList = function(arr, sorted) {
	sorted = sorted || false;
	var ul = document.createElement(sorted ? "ol" : "ul");
	for(var idx in arr) {
		var li = document.createElement("li");
		li.appendTextNode(arr[idx]);
		ul.appendChild(li);
	}
	return ul;
};

util.objectToSortedList = function(obj, attribs) {
	var keys = this.getKeysFromObject(obj);
	keys.sort();
	var ul = this.makeElement("ul", attribs);
	for(var idx in keys) {
		var li = document.createElement("li");
		var label = document.createElement("span");
		label.appendChild(document.createTextNode(keys[idx] + ":"));
		li.appendChild(label);
		li.appendChild(document.createTextNode(obj[keys[idx]]));
		ul.appendChild(li);
	}
	return ul;
};

util.getKeysFromObject = function(obj) {
	if(Object.keys)
		return Object.keys(obj);
	var ret = [];
	for(var key in obj)
		if(obj.hasOwnProperty(key))
			ret.push(key);
	return ret;
};

util.roundToDigit = function(val, decimals) {
	var b = Math.pow(10, decimals);
	return Math.round(val * b) / b;
};

var sizePostFixes = [" MB", " GB", " TB", " PB", " EB", " ZB", " YB"];

util.formatSizeMB = function(size) {
	if(size === 0)
		return "0 MB";
		
	var log = Math.log(size) / Math.log(1024);
	return this.roundToDigit((size / (Math.pow(1024, Math.floor(log)))), 3) + this.sizePostFixes[Math.ceil(log)-1];
};

util.makeSingletonGetter = function(object) {
	object.getInstance = function() {
		if(this.instance)
			return this.instance;
		else
			return this.instance = new this();
	};
};

util.makeAccessor = function(object, name, member, getter, setter) {
	if(getter !== true)
		object["set" + name] = getter ? getter : function(val) {
			this[member] = val;
		};
		
	if(setter !== true)
		object["get" + name] = setter ? setter : function() {
			return this[member];
		};
};

util.makeListAccessor = function(object, name, member) {
	object["add" + name] = function(val) {
		this[member].push(val);
		return val;
	};
	
	object["remove" + name] = function(val) {
		util.removeFromArray(this[member], val);
		return val;
	};
};

util.makeArrayGetter = function(object, name, member) {
	object["get" + name] = function() {
		return this[member].slice(0);
	};
};

util.makeGetByer = function(object, container, name, member) {
	object["getBy" + name] = function(value) {
		for(var idx in this[container])
			if(this[container][idx][member] == value)
				return this[container][idx];
	};
};

util.makeText = function(text) {
	return document.createTextNode(text || "");
};

util.isElement = function(obj) {
	try {
		return obj instanceof HTMLElement;
	} catch(e) {
		return (typeof obj==="object") &&
		(obj.nodeType === Node.ELEMENT_NODE) &&
		(typeof obj.style === "object") &&
		(typeof obj.ownerDocument === "object");
	}
};

/*
	Traverses the DOM sub-tree of the specified element or "document", calls the specified function for every element
	If the function returns any value the evaluates to false, the traverse will be cancelled
*/
util.domTraverse = function(elem, callback) {
	elem = elem || document;
	for(var child = elem.firstChild;child;child = child.nextSibling) {
		if(!this.isElement(child))
			continue;
		if(!callback(child))
			return;
	}
};

/*
	Traverses the DOM sub-tree of the specified element, calls the specified function for every element
	If the function returns any value the evaluates to true, the element will be present in the list returned by this
	function
*/
util.domTraverseGet = function(elem, callback) {
	var ret = [];
	for(var child = elem.firstChild;child;child = child.nextSibling) {
		if(!this.isElement(child))
			continue;
		if(callback(child))
			ret.push(child);
		ret = ret.concat(this.domTraverseGet(child, callback));
	}
	return ret;
}

var inputNames = [
	"input",
	"select",
	"textarea"
];

util.getFormValues = function(form) {
	var inputs = this.domTraverseGet(form, function(elem) { return util.inputNames.indexOf(elem.tagName.toLowerCase()) != -1; });
	var ret = {};
	for(var idx in inputs) {
		if(!inputs[idx].name)
			continue;
			
		switch(inputs[idx].type) {
			case "radio":
			case "checkbox":
				if(!inputs[idx].checked)
					continue;
		}
		
		ret[inputs[idx].name] = inputs[idx].value;
	}
	return ret;
};

util.deepCopyTo = function(dest, source) {
	for(var idx in source) {
		if(source[idx] === undefined)
			continue;
		if(source[idx] instanceof Object) {
			if(!dest[idx])
				dest[idx] = {};
			deepCopyTo(dest[idx], source[idx]);
		} else
			dest[idx] = source[idx];
	}
};

util.appendChilds = function(elem, childs) {
	for(var idx in childs)
		elem.appendChild(childs[idx]);
};

util.addEventListeners = function(elem, listeners) {
	for(var eventName in listeners) {
		if(listeners[eventName] instanceof Function)
			elem.addEventListener(eventName, listeners[eventName]);
		else
			elem.addEventListener(eventName, listeners[eventName].handler, listeners[eventName].capture);
	}
};

util.addEventListenerObject = function(elem, events, listener) {
	for(var eventName in events)
		elem.addEventListener(eventName, listener, listeners[eventName]);
};

util.makeElement = function(name, attributes, childs, events) {
	var elem = document.createElement(name);
	if(attributes)
		this.deepCopyTo(elem, attributes);
	
	if(childs)
		this.appendChilds(elem, childs);
		
	if(events)
		this.addEventListeners(elem, events);
			
	return elem;
};

util.makeElementTree = function(object, idMap) {
	if(object instanceof Node)
		return {node: object};
	idMap = idMap || {};
	var elem = document.createElement(object.tag);
	if(object.attributes)
		this.deepCopyTo(elem, object.attributes);
	if(object.id)
		idMap[object.id] = elem;
		
	if(object.children)
		object.children.forEach(function(childData) {
			var ret = util.makeElementTree(childData, idMap);
			elem.appendChild(ret.node);
		});
		
	if(object.events)
		this.addEventListeners(elem, object.events);
	return {
		node: elem,
		idMap: idMap
	};
}

util.makeSelect = function(attributes, options) {
	var select = this.makeElement("select", attributes);
	for(var idx in options)
		select.appendChild(this.makeElement("option", {
				value: options[idx].value,
				selected: options[idx].selected,
			}, 
			[this.makeText(options[idx].text)]
		));
	return select;
};

util.makeRecurseList = function(object, depthAttribs, depthPrefixes, depth) {
	depth = depth || 0;
	var ulElem = this.makeElement("ul", depthAttribs && depthAttribs[depth]);
	ulElem.classList.add("recursive-list-depth-" + depth);
	if(!(object instanceof Object))	
		return ulElem;
	
	if(object instanceof Array)
		for(var idx in object)
			ulElem.appendChild(this.makeElement("li", {}, [
				this.makeText(object[idx])
			]));
	else
		for(var idx in object)
			ulElem.appendChild(this.makeElement("li", {}, [
				util.makeText(((depthPrefixes && depthPrefixes[depth]) ? (depthPrefixes[depth] + ": " + idx) : idx + ":")),
				this.makeRecurseList(object[idx], depthAttribs, depthPrefixes, depth + 1)
			]));
	return ulElem;
}

util.makeToString = function(object, val) {
	return function() { return val; }
};

util.isNumber = function(val) {
	return isFinite(parseFloat(val));
};

util.copyTo = function(dest, source) {
	for(var idx in source)
		dest[idx] = source;
};

util.copyToNoOverride = function(dest, source) {
	for(var idx in source)
		if(!dest[idx])
			dest[idx] = source;
};

util.copyToRestricted = function(dest, source, what) {
	for(var idx in what)
		dest[what[idx]] = source[what[idx]];
	return dest;
};

util.makeInstanceDescriptorGetter = function(proto) {
	proto.getDescriptor = function() {
		var ret = [];
		for(idx in this.instances)
			ret.push(this.instances[idx].getDescriptor());
		return ret;
	};
};

util.arrayCopy = function(arr) {
	return arr.slice(0);
};

util.arrayFind = function(arr, func) {
	for(var idx in arr)
		if(func(arr[idx], idx))
			return arr[idx];
}

util.fetchResource = function(url, type, data) {
	return new Promise(function (resolve, reject) {
		var req = new XMLHttpRequest();
		req.open(data ? "POST" : "GET", url, true, "root", "root");
		if(type)
			req.responseType = type;
		req.addEventListener("readystatechange", function (event) {
			if (this.readyState === XMLHttpRequest.DONE && this.status === 200) {
				if(type)
					resolve(this.response);
				else
					resolve(this.responseText);
			} else if (this.status !== 200) {
				var err = new Error("Request failed");
				err.code = this.status;
				reject(err);
			}
		});
		req.timeout = 2000;
		req.addEventListener("timeout", function () {
			reject(new Error("Request timed out"));
		})
		req.send(data);
	});
};

util.fetchJSON = function(url, data) {
	return this.fetchResource(url, "json", data);
};

return util;