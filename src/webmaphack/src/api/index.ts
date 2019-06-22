export function api<T>(url: string): Promise<T> {
    return fetch(url)
        .then(response => {
            if (!response.ok) {
                console.log(`*********** error calling endpoint ${url}: statusText: ${response.statusText}`);
                throw new Error(response.statusText)
            }
            return response.json() as Promise<T>
        })
}