export default function transformArrayToObject2(data) {
    const output = {};

    // Loop through each object in the input data
    data.forEach((obj, i) => {
        // Loop through each key in the object
        Object.keys(obj).forEach((key) => {
            // If this is the first object, create a new array for this key in the output
            if (i === 0) {
                output[key] = {};
            }

            // Add the value for this key in this object to the corresponding array in the output
            output[key][i] = obj[key];
        });
    });

    return output;
}
