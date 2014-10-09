/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
"use strict";
var Util = {};

Util.makeSingletonGetter = function(classObject) {
	classObject.getInstance = function() {
		if(!classObject.instance)
			return classObject.instance = new classObject();
		return classObject.instance;
	};
};

Util.checkInterface = function(implementation, wantedInterface) {
	for(var idx in wantedInterface.methods) {
		if(!implementation.prototype[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx);
		if(implementation[idx] != wantedInterface.methods[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx + " correctly");
	}
};

Util.simulateClick = function(element) {
	var event = document.createEvent("MouseEvents"); 
	event.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null); 
	element.dispatchEvent(event);
};

Util.simulateFormSubmit = function(form) {
	var submitButton = Util.makeElement("input", {type: "submit"});
	this.simulateClick(form.appendChild(submitButton));
	form.removeChild(submitButton);
};

Util.enableEnterToSubmit = function(form) {
	form.appendChild(
		Util.makeElement("input", {
			type: "submit",
			style: "height: 0px; width: 0px; border: none; padding: 0px;",
			hidefocus: true
		})
	);
};

Util.preventFormSubmission = function(form) {
	form.addEventListener("submit", function(event) {
		event.preventDefault();
		event.stopPropagation();
		return false;
	}, true);
};

Util.checkAbstract = function(implementation, abstractParent) {
	for(var idx in abstractParent.abstracts) {
		if(!implementation.prototype[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx);
		if(implementation.prototype[idx].length != abstractParent.abstracts[idx])
			throw Error("Class " + implementation.name + " does not implement the method " + idx + " correctly");
	}
};

Util.removeFromArray = function(arr, obj) {
	arr.splice(arr.indexOf(obj), 1);
};

Util.getOccurencesInString = function(str, what) {
	return str.split(what).length - 1;
};

Util.arrayToList = function(arr, sorted) {
	sorted = sorted || false;
	var ul = document.createElement(sorted ? "ol" : "ul");
	arr.forEach(function(textNode) {
		var li = document.createElement("li");
		li.appendTextNode(textNode);
		ul.appendChild(li);
	});
	return ul;
};

Util.objectToSortedList = function(obj, attribs) {
	var keys = this.getKeysFromObject(obj);
	keys.sort();
	var ul = this.makeElement("ul", attribs);
	keys.forEach(function(key) {
		var li = document.createElement("li");
		var label = document.createElement("span");
		label.appendChild(document.createTextNode(key + ":"));
		li.appendChild(label);
		li.appendChild(document.createTextNode(obj[key]));
		ul.appendChild(li);
	});
	return ul;
};

Util.getKeysFromObject = function(obj) {
	if(Object.keys)
		return Object.keys(obj);
	var ret = [];
	for(var key in obj)
		if(obj.hasOwnProperty(key))
			ret.push(key);
	return ret;
};

Util.roundToDigit = function(val, decimals) {
	var b = Math.pow(10, decimals);
	return Math.round(val * b) / b;
};

var sizePostFixes = [" MB", " GB", " TB", " PB", " EB", " ZB", " YB"];

Util.formatSizeMB = function(size) {
	if(size === 0)
		return "0 MB";
		
	var log = Math.log(size) / Math.log(1024);
	return this.roundToDigit((size / (Math.pow(1024, Math.floor(log)))), 3) + this.sizePostFixes[Math.ceil(log)-1];
};

Util.makeSingletonGetter = function(object) {
	object.getInstance = function() {
		if(this.instance)
			return this.instance;
		else
			return this.instance = new this();
	};
};

Util.makeAccessor = function(object, name, member, getter, setter) {
	if(getter !== true)
		object["set" + name] = getter ? getter : function(val) {
			this[member] = val;
		};
		
	if(setter !== true)
		object["get" + name] = setter ? setter : function() {
			return this[member];
		};
};

Util.isString = function(val) {
    return typeof val === "string" || val instanceof String;
};

Util.makeListAccessor = function(object, name, member) {
	object["add" + name] = function(val) {
		this[member].push(val);
		return val;
	};
	
	object["remove" + name] = function(val) {
		Util.removeFromArray(this[member], val);
		return val;
	};
};

Util.makeArrayGetter = function(object, name, member) {
	object["get" + name] = function() {
		return this[member].slice(0);
	};
};

Util.makeGetByer = function(object, container, name, member) {
	object["getBy" + name] = function(value) {
		this[container].forEach(function(containerMember) {
			if(containerMember[member] == value)
				return containerMember;
		});
	};
};

Util.makeText = function(text) {
	return document.createTextNode(text || "");
};

Util.isElement = function(obj) {
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
Util.domTraverse = function(elem, callback) {
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
Util.domTraverseGet = function(elem, callback) {
	var ret = [];
	for(var child = elem.firstChild;child;child = child.nextSibling) {
		if(!this.isElement(child))
			continue;
		if(callback(child))
			ret.push(child);
		ret = ret.concat(this.domTraverseGet(child, callback));
	}
	return ret;
};

var inputNames = [
	"input",
	"select",
	"textarea"
];

Util.getFormValues = function(form) {
	var inputs = this.domTraverseGet(form, function(elem) { return Util.inputNames.indexOf(elem.tagName.toLowerCase()) != -1; });
	var ret = {};
	inputs.forEach(function(input) {
		if(!input.name)
			return;
			
		switch(input.type) {
			case "radio":
			case "checkbox":
				if(!input.checked)
					return;
		}
		
		ret[input.name] = input.value;
	});
	return ret;
};

Util.deepCopyTo = function(dest, source) {
	for(var idx in source) {
		if(source[idx] === undefined)
			continue;
		if(source[idx] instanceof Object) {
			if(!dest[idx])
				dest[idx] = {};
			this.deepCopyTo(dest[idx], source[idx]);
		} else
			dest[idx] = source[idx];
	}
};

Util.appendChilds = function(elem, childs) {
	childs.forEach(function(child) {
		elem.appendChild(child);
	});
};

Util.addEventListeners = function(elem, listeners) {
	for(var eventName in listeners) {
		if(listeners[eventName] instanceof Function)
			elem.addEventListener(eventName, listeners[eventName]);
		else
			elem.addEventListener(eventName, listeners[eventName].handler, listeners[eventName].capture);
	}
};

Util.addEventListenerObject = function(elem, events, listener) {
	for(var eventName in events)
		elem.addEventListener(eventName, listener, listeners[eventName]);
};

Util.makeElement = function(name, attributes, childs, events) {
	var elem = document.createElement(name);
	if(attributes)
		this.deepCopyTo(elem, attributes);
	
	if(childs)
		this.appendChilds(elem, childs);
		
	if(events)
		this.addEventListeners(elem, events);
			
	return elem;
};

Util.makeElementTree = function(object, idMap) {
	if(object == undefined) {
		return;
	}
	if(object instanceof Node)
		return {node: object};
	if(this.isString(object))
		return {node: this.makeText(object)};
	
	idMap = idMap || {};
	var elem = document.createElement(object.tag);
	if(object.attributes)
		this.deepCopyTo(elem, object.attributes);
	if(object.id)
		idMap[object.id] = elem;
		
	if(object.children) {
		if(object.children instanceof Array)
			object.children.forEach(function(childData) {
				var ret = Util.makeElementTree(childData, idMap);
				if(ret)
					elem.appendChild(ret.node);
			});
		else
			Util.makeElementTree(object.children, idMap)
	}
	if(object.events)
		this.addEventListeners(elem, object.events);
	return {
		node: elem,
		idMap: idMap
	};
};

Util.makeSelect = function(attributes, options, events) {
	var select = this.makeElement("select", attributes, undefined, events);
	options.forEach(function(option) {
		select.appendChild(this.makeElement("option", {
				value: option.value,
				selected: option.selected,
			}, 
			[this.makeText(option.text || option.value)]
		));
	}, this);
	return select;
};

Util.makeRecurseList = function(object, depthAttribs, depthPrefixes, depth) {
	depth = depth || 0;
	var ulElem = this.makeElement("ul", depthAttribs && depthAttribs[depth]);
	ulElem.classList.add("recursive-list-depth-" + depth);
	if(!(object instanceof Object))	
		return ulElem;
	
	if(object instanceof Array)
		object.forEach(function(subObject) {
			ulElem.appendChild(this.makeElement("li", {}, [
				this.makeText(subObject)
			]));
		}, this);
	else
		for(var idx in object)
			ulElem.appendChild(this.makeElement("li", {}, [
				Util.makeText(((depthPrefixes && depthPrefixes[depth]) ? (depthPrefixes[depth] + ": " + idx) : idx + ":")),
				this.makeRecurseList(object[idx], depthAttribs, depthPrefixes, depth + 1)
			]));
	return ulElem;
};

Util.makeToString = function(object, val) {
	return function() { return val; }
};

Util.isNumber = function(val) {
	return isFinite(parseFloat(val));
};

Util.copyTo = function(dest, source) {
	for(var idx in source)
		dest[idx] = source;
};

Util.copyToNoOverride = function(dest, source) {
	for(var idx in source)
		if(!dest[idx])
			dest[idx] = source;
};

Util.copyToRestricted = function(dest, source, what) {
	for(var idx in what)
		dest[what[idx]] = source[what[idx]];
	return dest;
};

Util.arrayCopy = function(arr) {
	return arr.slice(0);
};

Util.arrayFind = function(arr, func) {
	return arr.find(func);
};

Util.fetchResource = function(url, type, data) {
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
		req.send(JSON.stringify(data));
	});
};

Util.fetchJSON = function(url, data) {
	return this.fetchResource(url, "json", data);
};

Util.arrayOf = function(size, value) {
	var ret = [];
	for(var i = 0;i < size;++i)
		ret[i] = value;
	return ret;
};

Util.endsWith = function(subject, test) {
	return subject.lastIndexOf(test) == (subject.length - test.length);
};

Util.removeChildren = function(element) {
	while(element.firstChild)
		element.removeChild(myNode.firstChild);
};