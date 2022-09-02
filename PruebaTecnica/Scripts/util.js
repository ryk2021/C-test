const request = async ({ method = "GET", raw = null, URL = "", formData = false }) => {
    var requestOptions = {}
    switch (method) {
        case "POST":
            var myHeaders = new Headers();
            myHeaders.append("Content-Type", "application/json");
            requestOptions = {
                method: method,
                body: raw,
                redirect: 'follow',
                headers: myHeaders
            };
            break;
        case "GET":
            requestOptions = {
                method: 'GET',
                redirect: 'follow'
            };
            break;

        default:
            break;
    }

    formData ? (delete requestOptions['headers']) : requestOptions
    const response = await fetch(`${BASEURL}${URL}`, requestOptions)
    const isJson = response.headers.get('content-type')?.includes('application/json');
    const data = isJson ? await response.json() : null;

    if (!response.ok) {
        const error = (data && data.message) || response.status;
        return Promise.reject(error);
    }

    return data;
}