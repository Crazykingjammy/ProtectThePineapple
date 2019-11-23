Shader "Transparent/InvisibleShadowCaster" {
 
SubShader {

Usepass "VertexLit/SHADOWCOLLECTOR"
Usepass "VertexLit/SHADOWCASTER"

}
 
Fallback off
}