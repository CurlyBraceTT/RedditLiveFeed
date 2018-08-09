const path = require('path');
const webpack = require('webpack');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const bundleOutputDir = './wwwroot/dist';

var componentsPath = path.resolve(__dirname, './ClientApp/components');

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const mode = isDevBuild ? 'development' : 'production';

    return [{
        mode: mode,
        stats: { modules: false },
        entry: { 'main': './ClientApp/boot.tsx' },
        resolve: {
            extensions: ['.js', '.jsx', '.ts', '.tsx'],
            alias: {
                components: componentsPath
            },
        },
        output: {
            path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: 'dist/'
        },
        module: {
            rules: [
                { test: /\.tsx?$/, include: /ClientApp/, use: 'awesome-typescript-loader?silent=true' },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                {
                    test: /\.css$/,
                    use: [
                        {
                            loader: MiniCssExtractPlugin.loader,
                            options: {
                                // you can specify a publicPath here
                                // by default it use publicPath in webpackOptions.output
                                publicPath: '../'
                            }
                        },
                        "css-loader"
                    ]
                }
            ]
        },
        plugins: [
            new CheckerPlugin(),
            new MiniCssExtractPlugin({
                // Options similar to the same options in webpackOptions.output
                // both options are optional
                filename: "[name].css",
                chunkFilename: "[id].css"
            }),
            new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [
            // Plugins that apply in production builds only
            new webpack.optimize.UglifyJsPlugin(),
        ])
    }];
};