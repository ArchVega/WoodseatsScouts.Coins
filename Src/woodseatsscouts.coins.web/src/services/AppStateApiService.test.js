import AppStateApiService from "./AppStateApiService";

test('home page', async () => {
    console.log(await AppStateApiService().get())
})