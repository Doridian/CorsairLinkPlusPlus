	
var util = {};

util.arrayCopy = function(arr) {
	return arr.slice(0);
}

util.xmlHTTPPromise = function(url) {
	return new Promise(function (resolve, reject) {
		var req = new XMLHttpRequest();
		req.open("GET", url, true, "root", "root");
		req.responseType = "json";
		req.addEventListener("readystatechange", function (event) {
			if (this.readyState === XMLHttpRequest.DONE && this.status === 200 && this.response.success === true)
				resolve(this.response);
			else if (this.status !== 200) {
				var err = new Error("Request failed");
				err.code = this.status;
				reject(err);
			}
		});
		req.timeout = 2000;
		req.addEventListener("timeout", function () {
			reject(new Error("Request timed out"));
		})
		req.send();
	})
}

return util;