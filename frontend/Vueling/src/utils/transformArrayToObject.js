export default function transformArrayToObject(array) {
    console.log('hola')
    let result = {};
    for (let i = 0; i < array.length; i++) {
        for (let key in array[i]) {
            if (!result[key]) {
                result[key] = {};
            }
            result[key][i.toString()] = array[i][key];
        }
    }
    return result;
}