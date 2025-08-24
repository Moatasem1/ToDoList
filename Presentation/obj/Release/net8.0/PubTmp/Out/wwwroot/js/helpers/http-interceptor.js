axios.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        console.log(error);
        const returnedError = error.response?.data?.responseData.errors?.[0];

        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: returnedError.message,
            showCancelButton: false,
            showConfirmButton: false
        });
        return Promise.reject(error);
    }
);