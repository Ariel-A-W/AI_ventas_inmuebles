const GetItems = async (url: string) => {
  try {
    const response = await fetch(url);
    if (response.ok) {
      return await response.json();
    } else {
      console.error("Error en la API:", response.status);
      return null;
    }
  } catch (error) {
    console.error("Error en la petición:", error);
    return null;
  }
};

export default GetItems;