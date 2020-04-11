const Api = {
    post: (endpoint, content) => {
        return fetch(endpoint, 
            {
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    method: "POST",
                    body: JSON.stringify(content)
            })
            .then(response => response.json())
            .then(result => {
                if(result && result.status === 500){
                    throw new Error(result.detail);
                }
                return result;
            });
    }
};

export default Api;
