/************************************************************************** 

This is the Rollup.JS configuration file.

To get the typescript plugin going:

    cd into the wwwroot/js directory where this file is:

    npm install tslib --save-dev
    npm install typescript --save-dev
    npm install @rollup/plugin-typescript --save-dev
    npm install @types/bootstrap --save-dev 
    npm install @rollup/plugin-node-resolve --save-dev

***************************************************************************/

import typescript from '@rollup/plugin-typescript';
import { nodeResolve } from '@rollup/plugin-node-resolve';

class RollupConfig
{
    constructor(input, output)
    {
        this.input = input;

        this.external = ['bootstrap', 'jquery'];

        this.output = {
            /*            format: 'es',*/
            format: 'iife',
            compact: true,
            sourcemap: true,

            /*            dir: "dist",*/
            file: output,
            inlineDynamicImports: true,
            interop: "auto",
            globals: {
                bootstrap: 'bootstrap',
                jQuery: '$',
            }
        },
  
        this.plugins = [
            nodeResolve(),
            typescript(),
        ];
    }
}



const configs = [
    new RollupConfig('ts/pages/project/index.ts', 'dist/project.bundle.js'),
];



// rollup.config.js
export default configs;