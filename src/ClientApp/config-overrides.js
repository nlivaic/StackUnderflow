const { override } = require('customize-cra');
const cspHtmlWebpackPlugin = require("csp-html-webpack-plugin");

const cspConfigPolicy = {
    'default-src': "'self'",
    'connect-src': "http://localhost:3000/ https://localhost:5001/ https://localhost:6001/",
    'script-src': ["'self'"],
    'style-src': ["'self'"],
    'frame-src': "https://localhost:6001/ http://localhost:3000/",
};

function addCspHtmlWebpackPlugin(config) {
    if (process.env.NODE_ENV === 'production') {
        config.plugins.push(new cspHtmlWebpackPlugin(cspConfigPolicy, {
            enabled: true,
            hashingMethod: 'sha256',
            hashEnabled: {
                'script-src': false,
                'style-src': false
            },
            nonceEnabled: {
                'script-src': false,
                'style-src': false
            },
        }));
    }

    return config;
}

module.exports = {
    webpack: override(addCspHtmlWebpackPlugin),
};